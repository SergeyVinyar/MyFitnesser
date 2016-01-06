namespace MyFitnesser.Droid.Controls {
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Text;

  using Android.App;
  using Android.Graphics;
  using Android.Content;
  using Android.OS;
  using Android.Runtime;
  using Android.Util;
  using Android.Views;
  using Android.Widget;

  using Core.ViewModels;


  public class CalendarInnerView: View {

    public CalendarInnerView(Context context, Android.Util.IAttributeSet attrs, int defStyle): base(context, attrs, defStyle) { }

    public CalendarInnerView(Context context, Android.Util.IAttributeSet attrs): base(context, attrs) { }

    public CalendarInnerView(Context context): base(context) { }

    public DateTime Date { 
      set {
        _Date = value;
        Invalidate();
      }
      get {
        return _Date;
      }
    }
    private DateTime _Date;

    public ObservableCollection<CalendarDayViewModel.Train> Trains { 
      set {
        _Trains = value;
        Invalidate();
      }
      get {
        return _Trains;
      }
    }
    private ObservableCollection<CalendarDayViewModel.Train> _Trains;

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
      _Width = MeasureSpec.GetSize(widthMeasureSpec);

      _TimePaint.Color = Color.Black;
      _TimePaint.AntiAlias = true;
      _TimePaint.TextSize = _Width / 10 / 3;
      _TimePaint.TextAlign = Paint.Align.Right;

      _PageTitlePaint.Color = Color.Black;
      _PageTitlePaint.AntiAlias = true;
      _PageTitlePaint.TextSize = _TimePaint.TextSize;
      _PageTitlePaint.TextAlign = Paint.Align.Center;

      _PageTitleHeight = (int)_PageTitlePaint.TextSize * 3;

      MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);
      _Height = MeasureSpec.GetSize(heightMeasureSpec);

      if (heightMode == MeasureSpecMode.Exactly) {
        // _Height = _Height;
      } else if (heightMode == MeasureSpecMode.AtMost) {
        _Height = Math.Min(_Width / 8 * 24 + _PageTitleHeight, _Height);
      } else { // Unspecified
        _Height = _Width / 8 * 24 + _PageTitleHeight;
      }

      SetMeasuredDimension(_Width, _Height);

      _HGap = _Width / 5;
      _VGap = (_Height - _PageTitleHeight) / 24;

      _LinesPaint.Color = Color.Black;

      _RectPaint.Color = Color.Silver;
      _RectPaint.Alpha = 50;

      _RectStrokePaint.Color = Color.Silver;
      _RectStrokePaint.Alpha = 230;

      _TrainTitlePaint.Color = Color.Black;
      _TrainTitlePaint.AntiAlias = true;
      _TrainTitlePaint.TextSize = _TimePaint.TextSize;
      _TrainTitlePaint.TextAlign = Paint.Align.Left;

      _TimeLinePaint.Color = Color.Red;
      _TimeLinePaint.AntiAlias = true;
    }

    protected override void OnDraw(Canvas canvas) {
      canvas.DrawColor(Color.White);

      // Рисуем заголовок страницы
      canvas.DrawText(Date.ToLongDateString(),   _Width / 2, (int)(_PageTitlePaint.TextSize * 1.5),  _PageTitlePaint);
      canvas.DrawText(Date.DayOfWeek.ToString(), _Width / 2, (int)(_PageTitlePaint.TextSize * 3), _PageTitlePaint);

      // Рисуем сетку
      for (int i = 1; i < 24; i++) {
        canvas.DrawLine(_HGap, _VGap * i + _PageTitleHeight, MeasuredWidth, _VGap * i + _PageTitleHeight, _LinesPaint);
        canvas.DrawText(i.ToString() + ":00", _HGap - 10, _VGap * i + _PageTitleHeight + _TimePaint.TextSize / 3, _TimePaint);
      }

      // Рисуем события
      if (Trains != null) {
        var trains = Trains.OrderBy(_ => _.StartDate).ToArray();
        for (int i = 0; i < trains.Length; i++) {
          canvas.DrawRect(new Rect(_HGap + 10, GetVPosOfTime(trains[i].StartDate), MeasuredWidth, GetVPosOfTime(trains[i].EndDate)), _RectPaint);
          canvas.DrawRect(new Rect(_HGap + 10, GetVPosOfTime(trains[i].StartDate), _HGap + 13, GetVPosOfTime(trains[i].EndDate)), _RectStrokePaint);
          canvas.DrawText(trains[i].ClientName, _HGap + 10 + 8, GetVPosOfTime(trains[i].StartDate) + 5 + _TrainTitlePaint.TextSize, _TrainTitlePaint);
        }
      }

      // Линия текущего времени
      if (DateTime.Now.Date == Date) {
        canvas.DrawLine(_HGap / 5, GetVPosOfTime(DateTime.Now), MeasuredWidth, GetVPosOfTime(DateTime.Now), _TimeLinePaint);
        canvas.DrawCircle(_HGap / 3, GetVPosOfTime(DateTime.Now), _HGap / 14, _TimeLinePaint);
      }
    }

    private int GetVPosOfTime(DateTime time) {
      var result = _PageTitleHeight;
      result += _VGap * time.Hour;
      result += (int)((double)_VGap / 60 * time.Minute);
      return result;
    }

    private Paint _LinesPaint = new Paint();
    private Paint _TimePaint = new Paint();
    private Paint _PageTitlePaint = new Paint();
    private Paint _RectPaint = new Paint();
    private Paint _RectStrokePaint = new Paint();
    private Paint _TrainTitlePaint = new Paint();
    private Paint _TimeLinePaint = new Paint();

    private int _HGap;
    private int _VGap;
    private int _PageTitleHeight;
    private int _Width;
    private int _Height;

  }

}

