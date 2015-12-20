namespace MyFitnesser.Droid.Views {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using Android;
  using Android.App;
  using Android.Content;
  using Android.OS;
  using Android.Runtime;
  using Android.Util;
  using Android.Views;
  using Android.Widget;

  using Android.Graphics.Drawables;
  using Android.Graphics.Drawables.Shapes;
  using Android.Graphics;

  using Cirrious.MvvmCross.Droid.Fragging.Fragments;


  public class CalendarView : MvxFragment {
  
    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      return inflater.Inflate(MyFitnesser.Droid.Resource.Layout.Calendar, container, false);
    }


    public override void OnResume() {
      base.OnResume();
    }

  }

  public class CalendarInnerView: View {

    public CalendarInnerView(Context context, Android.Util.IAttributeSet attrs, int defStyle): base(context, attrs, defStyle) {

    }

    public CalendarInnerView(Context context, Android.Util.IAttributeSet attrs): base(context, attrs) {
    
    }

    public CalendarInnerView(Context context): base(context) {
    
    }

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
      //var height = MeasuredWidth / 5 * 24;
      //SetMeasuredDimension(MeasuredWidth, height);

      int widthSize = MeasureSpec.GetSize(widthMeasureSpec);

      MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);
      int heightSize = MeasureSpec.GetSize(heightMeasureSpec);

      int height;
      if (heightMode == MeasureSpecMode.Exactly) {
        height = heightSize;
      } else if (heightMode == MeasureSpecMode.AtMost) {
        height = Math.Min(widthSize / 8 * 24, heightSize);
      } else { // Unspecified
        height = widthSize / 8 * 24;
      }

      SetMeasuredDimension(widthSize, height);

      _HGap = widthSize / 5;
      _VGap = height / 24;

      _LinesPaint.Color = Color.Black;

      _TimePaint.Color = Color.Black;
      _TimePaint.AntiAlias = true;
      _TimePaint.TextSize = widthSize / 10 / 3;
      _TimePaint.TextAlign = Paint.Align.Right;

      _RectPaint.Color = Color.Blue;
      _RectPaint.Alpha = 50;

      _RectStrokePaint.Color = Color.Blue;
      _RectStrokePaint.Alpha = 230;

      _TitlePaint.Color = Color.Blue;
      _TitlePaint.AntiAlias = true;
      _TitlePaint.TextSize = _TimePaint.TextSize;
      _TitlePaint.TextAlign = Paint.Align.Left;
    }

    protected override void OnDraw(Canvas canvas) {
      canvas.DrawColor(Color.White);

      // Рисуем сетку
      for (int i = 1; i < 24; i++) {
        canvas.DrawLine(_HGap, _VGap * i, MeasuredWidth, _VGap * i, _LinesPaint);
        canvas.DrawText(i.ToString() + ":00", _HGap - 10, _VGap * i + _TimePaint.TextSize / 3, _TimePaint);
      }

      // Рисуем событие
      canvas.DrawRect(new Rect(_HGap + 10, _VGap * 12 + 10, MeasuredWidth, _VGap * 13 + 10), _RectPaint);
      canvas.DrawRect(new Rect(_HGap + 10, _VGap * 12 + 10, _HGap + 13, _VGap * 13 + 10), _RectStrokePaint);

      canvas.DrawText("Залипукин Иван Дормидонтович", _HGap + 10 + 8, _VGap * 12 + 10 + 5 + _TitlePaint.TextSize, _TitlePaint);
    }

    private Paint _LinesPaint = new Paint();
    private Paint _TimePaint = new Paint();
    private Paint _RectPaint = new Paint();
    private Paint _RectStrokePaint = new Paint();
    private Paint _TitlePaint = new Paint();

    private int _HGap;
    private int _VGap;

  }

}

