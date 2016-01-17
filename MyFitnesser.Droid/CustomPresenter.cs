namespace MyFitnesser.Droid {
  using System;
  using System.Collections.Generic;

  using Cirrious.MvvmCross.Droid.Views;
  using Cirrious.MvvmCross.ViewModels;


  public class CustomPresenter: MvxAndroidViewPresenter, ICustomPresenter {

    public override void Show(MvxViewModelRequest request) {
      var host = this.Activity as IFragmentHost;
      if (host != null && _ViewTypes.Contains(request.ViewModelType))
        host.Show(request);
      else
        base.Show(request);
    }

    public override void ChangePresentation(MvxPresentationHint hint) {
      // TODO Счас не проверяется, что закрывают именно тот фрагмент,
      // который находится на вершине стека
      var host = this.Activity as IFragmentHost;      
      if (host != null && hint is MvxClosePresentationHint)
        host.GoBack();
      else
        base.ChangePresentation(hint);
    }

    public void RegisterFragment(Type viewModelType) {
      _ViewTypes.Add(viewModelType);
    }

    private HashSet<Type> _ViewTypes = new HashSet<Type>();
  }

  /// <summary>Для регистрации фрагментов</summary>
  public interface ICustomPresenter {
    void RegisterFragment(Type viewModelType);
  }

  /// <summary>Имплементирует хост фрагментов</summary>
  public interface IFragmentHost {
    bool Show(MvxViewModelRequest request);
    void GoBack();
  }
}