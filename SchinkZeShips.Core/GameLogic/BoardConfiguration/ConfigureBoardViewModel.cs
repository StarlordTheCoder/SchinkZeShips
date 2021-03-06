﻿using System;
using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.GameLobby;
using SchinkZeShips.Core.GameLogic.Board;
using SchinkZeShips.Core.GameLogic.InGame;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic.BoardConfiguration
{
	public class ConfigureBoardViewModel : ViewModelBase
	{
		private Game _currentGame;

		private CellViewModel _firstClickedCell;

		public ConfigureBoardViewModel()
		{
			LockInLayoutCommand = new Command(LockInLayoutAsync,
				() => ConfiguringBoard.Cells.SelectMany(c => c).Count(c => c.Ship != null) ==
				      BoardStateViewModel.AmountOfCellsWithShips);
			ConfiguringBoard = new BoardStateViewModel(new BoardState(), true, true);
		}

		public Command LockInLayoutCommand { get; }

		public Game CurrentGame
		{
			get => _currentGame;
			set
			{
				_currentGame = value;
				OnPropertyChanged();
			}
		}

		public BoardStateViewModel ConfiguringBoard { get; }

		private async void CellSelectedChanged(object sender, EventArgs eventArgs)
		{
			var clickedCell = (CellViewModel) sender;

			if (clickedCell.IsSelected && clickedCell.Ship != null)
			{
				if (_firstClickedCell != null)
				{
					_firstClickedCell.IsSelected = false;
				}
				clickedCell.IsSelected = false;

				var remove =
					await Dialogs.ConfirmAsync(
						$"Möchten Sie dieses {(clickedCell.Ship.ShipType.HasValue ? (int) clickedCell.Ship.ShipType.Value : 0)}er Schiff entfernen?", "Entfernen bestätigen", "Ja", "Nein");

				if (remove)
				{
					ConfiguringBoard.RemoveShip(clickedCell.Ship);

					LockInLayoutCommand.ChangeCanExecute();
				}

				return;
			}

			if (!clickedCell.IsSelected)
			{
				_firstClickedCell = null;

				return;
			}

			if (_firstClickedCell == null)
			{
				_firstClickedCell = clickedCell;
			}
			else
			{
				var firstCoordinate = _firstClickedCell.Coordinate;
				var secondCoordinate = clickedCell.Coordinate;

				_firstClickedCell.IsSelected = false;
				clickedCell.IsSelected = false;

				_firstClickedCell = null;

				if (firstCoordinate == null || secondCoordinate == null)
					throw new Exception("You clicked cells which are outside of the field. This should never happen");

				var shipToBePlaced = new List<Coordinate>();

				if (firstCoordinate.Row == secondCoordinate.Row)
				{
					int biggerColumn;
					int smallerColumn;
					if (firstCoordinate.Column > secondCoordinate.Column)
					{
						biggerColumn = firstCoordinate.Column;
						smallerColumn = secondCoordinate.Column;
					}
					else
					{
						smallerColumn = firstCoordinate.Column;
						biggerColumn = secondCoordinate.Column;
					}

					for (var column = smallerColumn; column <= biggerColumn; column++)
						shipToBePlaced.Add(new Coordinate(firstCoordinate.Row, column));
				}
				else if (firstCoordinate.Column == secondCoordinate.Column)
				{
					int biggerRow;
					int smallerRow;
					if (firstCoordinate.Row > secondCoordinate.Row)
					{
						biggerRow = firstCoordinate.Row;
						smallerRow = secondCoordinate.Row;
					}
					else
					{
						smallerRow = firstCoordinate.Row;
						biggerRow = secondCoordinate.Row;
					}

					for (var row = smallerRow; row <= biggerRow; row++)
						shipToBePlaced.Add(new Coordinate(row, firstCoordinate.Column));
				}
				else
				{
					Dialogs.Alert("Das Schiff darf nicht diagonal platziert werden", "Platzieren fehlgeschlagen");
					return;
				}

				var addedShip = ConfiguringBoard.TryAddShip(new Ship(
					shipToBePlaced.OrderBy(s => s.Row).ThenBy(s => s.Column).Select(s => ConfiguringBoard.Cells[s.Row][s.Column])
						.ToList(), true, Guid.NewGuid().ToString()));

				if (!addedShip)
					Dialogs.Alert("Das Schiff kann dort nicht platziert werden", "Platzieren fehlgeschlagen");
				else
					LockInLayoutCommand.ChangeCanExecute();
			}
		}

		public override void OnAppearing()
		{
			base.OnAppearing();

			foreach (var cell in ConfiguringBoard.Cells.SelectMany(c => c))
				cell.SelectedChanged += CellSelectedChanged;
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();

			foreach (var cell in ConfiguringBoard.Cells.SelectMany(c => c))
				cell.SelectedChanged -= CellSelectedChanged;
		}

		private async void LockInLayoutAsync()
		{
			ShowLoading("Bestätige Layout");

			var latestGameState = await Service.GetCurrentGame();

			if (latestGameState == null)
			{
				PushViewModal(new StartView());
				return;
			}
			if (latestGameState.RunningGameState == null)
			{
				PushViewModal(new GameLobbyView(latestGameState));
				return;
			}

			if (latestGameState.ThisPlayerIsGameCreator())
				latestGameState.RunningGameState.BoardCreator = ConfiguringBoard.Model;
			else
				latestGameState.RunningGameState.BoardParticipant = ConfiguringBoard.Model;

			await Service.UpdateGameState(latestGameState.Id, latestGameState.RunningGameState);

			HideLoading();

			//TODO Use Service / better class
			GameExtensions.ResetBoards();

			PushViewModal(new InGameView(latestGameState));
		}
	}
}