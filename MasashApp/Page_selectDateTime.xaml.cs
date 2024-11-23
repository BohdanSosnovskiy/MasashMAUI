using MasashApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MasashApp;

public partial class Page_selectDateTime : ContentPage, INotifyPropertyChanged
{
	public Page_selectDateTime()
	{
		InitializeComponent();
        CalendarDates = new List<Model_Calendar_Item>();
        Loaded += Page_selectDateTime_Loaded;
        selected_day = new Model_Calendar_Item();
        Morning = new List<Model_TimesItem>();
        Day = new List<Model_TimesItem>();
        Afterun = new List<Model_TimesItem>();
        BindingContext = this;
        IsViewTimes = false;
        IsViewMorning = false;
        IsViewDay = false;
        IsViewAfterun = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool isViewTimes;
    public bool IsViewTimes
    {
        get
        {
            return isViewTimes;
        }
        set
        {
            isViewTimes = value;
            OnPropertyChanged();
        }
    }

    private bool isViewMorning;
    public bool IsViewMorning
    {
        get
        {
            return isViewMorning;
        }
        set
        {
            isViewMorning = value;
            OnPropertyChanged();
        }
    }

    private bool isViewDay;
    public bool IsViewDay
    {
        get
        {
            return isViewDay;
        }
        set
        {
            isViewDay = value;
            OnPropertyChanged();
        }
    }

    private bool isViewAfterun;
    public bool IsViewAfterun
    {
        get
        {
            return isViewAfterun;
        }
        set
        {
            isViewAfterun = value;
            OnPropertyChanged();
        }
    }

    private void Page_selectDateTime_Loaded(object? sender, EventArgs e)
    {
        InitCalendar(DateTime.Now);
    }

    public List<Model_Calendar_Item> CalendarDates;
    public DateTime TargetDate;
    public Model_Calendar_Item selected_day;

    public void InitCalendar(DateTime date)
    {
        DateTime current = DateTime.Now;
        TargetDate = date;
        
        LabelMouth.Text = GetMounth(date);
        CalendarDates = new List<Model_Calendar_Item>();

        Model_Master? selectedMaster = StaticData.linkMainPage.SelectedMaster;

        var CountDays = DateTime.DaysInMonth(date.Year, date.Month);
        for (int i = 0; i < CountDays; i++) 
        {
            var item = new Model_Calendar_Item();
            var item_date = new DateTime(date.Year, date.Month, i + 1);
            item.Date = item_date;
            if (current.Day == item_date.Day && current.Month == item_date.Month && current.Year == item_date.Year)
            {
                item.IsCurrentDay = true;
            }
            else
            {
                item.IsCurrentDay = false;
            }
            if (selected_day.Date.Day == item_date.Day && selected_day.Date.Month == item_date.Month && selected_day.Date.Year == item_date.Year)
            {
                item.IsSelectedDay = true;
            }
            else
            {
                item.IsSelectedDay = false;
            }
            
            if(selectedMaster != null)
            {
                var Schedules = selectedMaster.IsFoundDateInSchedules(item.Date);
                if (Schedules != null)
                {
                    item.Schedule = Schedules;
                    item.IsFoudTime = true;
                }
                else
                {
                    item.Schedule = null;
                    item.IsFoudTime = false;
                }
            }
            
            CalendarDates.Add(item);
        }
        
        int First_DayWeek = Convert.ToInt32(CalendarDates[0].Date.DayOfWeek);
        int index_day = 0;

        for (int i = 0; i < Grid_labesDate.Children.Count; i++)
        {
            Frame frame = (Frame)Grid_labesDate.Children[i];
            Label label = (Label)frame.Content;
            if (i < First_DayWeek-1)
            {
                frame.IsVisible = false;
            }
            else
            {
                if(index_day < CalendarDates.Count)
                {
                    frame.IsVisible = true;
                    
                    label.Text = CalendarDates[index_day].Date.Day.ToString();

                    if(CalendarDates[index_day].IsCurrentDay)
                    {
                        frame.BorderColor = Colors.Black;
                    }
                    else
                    {
                        frame.BorderColor = Colors.Transparent;
                    }

                    if(CalendarDates[index_day].IsSelectedDay)
                    {
                        frame.BackgroundColor = Colors.Black;
                        label.TextColor = Colors.White;
                    }
                    else
                    {
                        frame.BackgroundColor= Colors.Transparent;
                        label.TextColor = Colors.Gray;
                    }

                    if (CalendarDates[index_day].IsFoudTime)
                    {
                        label.TextColor = Colors.Black;
                    }
                    CalendarDates[index_day].Frame_control = frame;
                    CalendarDates[index_day].Label_control = label;
                    index_day++;
                }
                else
                {
                    frame.IsVisible = false;
                }
            }
        }
    }

    List<Model_TimesItem> Morning;
    List<Model_TimesItem> Day;
    List<Model_TimesItem> Afterun;
    public void InitTimesCalendar(List<Model_TimesItem> timesItems)
    {
        Morning = new List<Model_TimesItem>();
        Day = new List<Model_TimesItem>();
        Afterun = new List<Model_TimesItem>();

        IsViewTimes = true;

        for (int i = 0; i < timesItems.Count; i++)
        {
            if (timesItems[i].Time.Hour < 12)
            {
                Morning.Add(timesItems[i]);
            }
            else if(timesItems[i].Time.Hour >= 12 && timesItems[i].Time.Hour < 19)
            {
                Day.Add(timesItems[i]);
            }
            else if(timesItems[i].Time.Hour >= 19)
            {
                Afterun.Add(timesItems[i]);
            }
        }

        if(Morning.Count > 0)
        {
            IsViewMorning = true;
        }
        else
        {
            IsViewMorning = false;
        }

        if(Day.Count > 0)
        {
            IsViewDay = true;
        }
        else
        {
            IsViewDay = false;
        }

        if(Afterun.Count > 0)
        { 
            IsViewAfterun = true; 
        }
        else
        {
            IsViewAfterun = false;
        }

        Grid_morning.Children.Clear();
        Grid_morning.AddRowDefinition(new RowDefinition());
        //< Frame Padding = "0" Grid.Column = "0" BorderColor = "Gray" BackgroundColor = "Transparent" Margin = "5" CornerRadius = "0" >
        //    < Label Text = "11:00" FontSize = "22" TextColor = "Black" HorizontalOptions = "Center" VerticalOptions = "Center" />
        //</ Frame >
        int Cols_Count = 4;
        int Currnet_Col = 0;
        int Current_Row = 0;

        for (int i = 0; i < Morning.Count; i++) 
        {
            if(Currnet_Col < Cols_Count)
            {
                Morning[i].Frame_layout = AddTime_Layout(Grid_morning, Morning[i], Currnet_Col, Current_Row);
                Currnet_Col++;
            }
            else
            {
                Currnet_Col = 0;
                Current_Row++;
                Grid_morning.AddRowDefinition(new RowDefinition());
                Morning[i].Frame_layout = AddTime_Layout(Grid_morning, Morning[i], Currnet_Col, Current_Row);
                Currnet_Col++;
            }
        }

        Grid_Day.Clear();
        Grid_Day.AddRowDefinition(new RowDefinition());
        Currnet_Col = 0;
        Current_Row = 0;

        for (int i = 0; i < Day.Count; i++)
        {
            if (Currnet_Col < Cols_Count)
            {
                Day[i].Frame_layout = AddTime_Layout(Grid_Day, Day[i], Currnet_Col, Current_Row);
                Currnet_Col++;
            }
            else
            {
                Currnet_Col = 0;
                Current_Row++;
                Grid_morning.AddRowDefinition(new RowDefinition());
                Day[i].Frame_layout = AddTime_Layout(Grid_Day, Day[i], Currnet_Col, Current_Row);
                Currnet_Col++;
            }
        }

        Grid_Afterun.Clear();
        Grid_Afterun.AddRowDefinition(new RowDefinition());
        Currnet_Col = 0;
        Current_Row = 0;

        for (int i = 0; i < Afterun.Count; i++)
        {
            if (Currnet_Col < Cols_Count)
            {
                Afterun[i].Frame_layout = AddTime_Layout(Grid_Afterun, Afterun[i], Currnet_Col, Current_Row);
                Currnet_Col++;
            }
            else
            {
                Currnet_Col = 0;
                Current_Row++;
                Grid_morning.AddRowDefinition(new RowDefinition());
                Afterun[i].Frame_layout = AddTime_Layout(Grid_Afterun, Afterun[i], Currnet_Col, Current_Row);
                Currnet_Col++;
            }
        }
    }
    public void FrameTime_Tap(object sender, TappedEventArgs e)
    {
        bool isFound = false;
        Frame item = (Frame)sender;

        for (int i = 0; i < Morning.Count; i++)
        {
            if (Morning[i].Frame_layout.Id == item.Id)
            {
                isFound = true;
                StaticData.linkMainPage.SelectedTime = Morning[i].Time;
                StaticData.linkMainPage.IsHideTime = false;
                StaticData.linkMainPage.IsVisibleTime = true;
                Close();
                break;
            }
        }
        if (!isFound)
        {
            for (int i = 0; i < Day.Count; i++)
            {
                if (Day[i].Frame_layout.Id == item.Id)
                {
                    isFound = true;
                    StaticData.linkMainPage.SelectedTime = Day[i].Time;
                    StaticData.linkMainPage.IsHideTime = false;
                    StaticData.linkMainPage.IsVisibleTime = true;
                    Close();
                    break;
                }
            }
        }
        if (!isFound)
        {
            for (int i = 0; i < Afterun.Count; i++)
            {
                if (Afterun[i].Frame_layout.Id == item.Id)
                {
                    isFound = true;
                    StaticData.linkMainPage.SelectedTime = Afterun[i].Time;
                    StaticData.linkMainPage.IsHideTime = false;
                    StaticData.linkMainPage.IsVisibleTime = true;
                    Close();
                    break;
                }
            }
        }
    }

    public Frame AddTime_Layout(Grid target, Model_TimesItem TimeItem, int Currnet_Col, int Current_Row)
    {
        Frame item_frame = new Frame();
        item_frame.Padding = new Thickness(0);
        item_frame.SetValue(Grid.ColumnProperty, 0);
        item_frame.BorderColor = Colors.Gray;
        item_frame.BackgroundColor = Colors.Transparent;
        item_frame.Margin = new Thickness(5);
        item_frame.VerticalOptions = LayoutOptions.Fill;
        item_frame.HorizontalOptions = LayoutOptions.Fill;
        item_frame.HeightRequest = 30;
        item_frame.CornerRadius = 0;
        var tap = new TapGestureRecognizer();
        tap.Tapped += FrameTime_Tap;
        item_frame.GestureRecognizers.Add(tap);

        Label item_label = new Label();
        item_label.Text = GetTime(TimeItem.Time);
        item_label.FontSize = 22;
        item_label.TextColor = Colors.Black;
        item_label.HorizontalOptions = LayoutOptions.Center;
        item_label.VerticalOptions = LayoutOptions.Center;

        item_frame.Content = item_label;
        target.Add(item_frame, Currnet_Col, Current_Row);
        return item_frame;
    }

    public string GetMounth(DateTime date)
    {
        switch (date.Month)
        {
            case 1: return "Январь";
            case 2: return "Февраль";
            case 3: return "Март";
            case 4: return "Апрель";
            case 5: return "Май";
            case 6: return "Июнь";
            case 7: return "Июль";
            case 8: return "Август";
            case 9: return "Сентябрь";
            case 10: return "Октябрь";
            case 11: return "Ноябрь";
            case 12: return "Декабрь";
            default: return "";
        }
    }
     
    public string GetTime(DateTime date) 
    {
        string result = "";
        int Hours = date.Hour;
        int Minute = date.Minute;

        if (Hours < 10)
        {
            result += $"0{Hours}";
        }
        else
        {
            result += $"{Hours}";
        }

        result += ":";

        if (Minute < 10)
        {
            result += $"0{Minute}";
        }
        else
        {
            result += $"{Minute}";
        }
        return result;
    }

    public void ItemCalendar_Touch(object sender, TappedEventArgs e)
    {
        Frame? frame = sender as Frame;
        if (frame != null)
        {
            Guid? id = frame.Id;
            for (int i = 0; i < CalendarDates.Count; i++)
            {
                var CalendarDate = CalendarDates[i];
                if (CalendarDate.Frame_control.Id == id)
                {
                    CalendarDate.Frame_control.BackgroundColor = Colors.Black;
                    CalendarDate.Label_control.TextColor = Colors.White;
                    CalendarDate.IsSelectedDay = true;
                    selected_day = CalendarDate;
                    if (CalendarDate.IsFoudTime)
                    {
                        InitTimesCalendar(CalendarDate.Schedule.Times);
                    }
                }
                else
                {
                    CalendarDate.Frame_control.BackgroundColor = Colors.Transparent;
                    if (CalendarDate.IsFoudTime)
                    {
                        CalendarDate.Label_control.TextColor = Colors.Black;
                    }
                    else
                    {
                        CalendarDate.Label_control.TextColor = Colors.Gray;
                    }
                    CalendarDate.IsSelectedDay = false;
                }
            }
        }
    }

    public void Next_Mounth(object sender, TappedEventArgs e)
    {
        DateTime next = TargetDate.AddMonths(1);
        InitCalendar(next);
    }

    public void Prew_Mounth(object sender, TappedEventArgs e)
    {
        DateTime prew = TargetDate.AddMonths(-1);
        InitCalendar(prew);
    }

    public void Back_Touch(object sender, TappedEventArgs e)
    {
        Close();
    }

    protected override bool OnBackButtonPressed()
    {
        Close();
        return true;
    }

    public async void Close()
    {
        await Navigation.PopModalAsync(animated: true);
    }
}