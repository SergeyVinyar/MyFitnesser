namespace MyFitnesser.Core.ViewModels {
  using System;
  using Cirrious.MvvmCross.ViewModels;

  public class MenuViewModel : MvxViewModel {

    public IMvxCommand ShowCalendarDaysCommand {
      get { return new MvxCommand(ShowCalendarDays); }
    }

    private void ShowCalendarDays() {
      ShowViewModel<CalendarDaysViewModel>();
    }

    public IMvxCommand ShowCalendarYearsCommand {
      get { return new MvxCommand(ShowCalendarYears); }
    }

    private void ShowCalendarYears() {
      ShowViewModel<CalendarYearsViewModel>();
    }

    public IMvxCommand ShowClientsCommand {
      get { return new MvxCommand(ShowClients); }
    }

    private void ShowClients() {
      ShowViewModel<ClientsViewModel>();
    }

  }
}