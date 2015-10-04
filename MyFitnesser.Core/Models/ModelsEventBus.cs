namespace MyFitnesser.Core {
  using System;
  using DSoft.Messaging;


  /// <summary>События добавления/удаления моделей</summary>
  /// <remarks>Используются презентерами, работающими со списками моделей</remarks>
  internal class ModelsMessageBus {

    public static MessageBus Get() {
      if (_MessageBus == null)
        _MessageBus = new MessageBus();
      return _MessageBus;
    }

    private static MessageBus _MessageBus;
  }
}