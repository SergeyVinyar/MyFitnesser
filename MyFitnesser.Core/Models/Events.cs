namespace MyFitnesser.Core {
  using System;

  public class ModelEventArgs : EventArgs {
    
    public ModelEventArgs(Guid id) {
      Id = id;
    }

    public readonly Guid Id;
  }

  public delegate void ModelEventHandler(object sender, ModelEventArgs e);
}

