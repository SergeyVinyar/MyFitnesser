namespace MyFitnesser.Core.Utils {
  using System;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;


  public class SuspendableObservableCollection<T> : ObservableCollection<T> {

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
      if (!_Suspended)
        base.OnCollectionChanged(e);
    }

    public void Suspend() {
      _Suspended = true;
    }

    public void Resume() {
      _Suspended = false;
    }

    public void ResumeAndNotify() {
      _Suspended = false;
      OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private bool _Suspended = false;
  }
}

