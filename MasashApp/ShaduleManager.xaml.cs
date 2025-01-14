using CommunityToolkit.Maui.Behaviors;
using MasashApp.Models;
using System.Diagnostics.Metrics;

namespace MasashApp;

public partial class ShaduleManager : ContentPage
{
    public Model_schedule Current_shedule { get; set; }
    /// <summary>
    /// Выбранная дата
    /// </summary>
    public DateTime SelectedDate { get; set; }
    /// <summary>
    /// Список дат для нижнего календаря
    /// </summary>
    List<ItemGridWeek> dateTimesWeeks { get; set; }
    public List<Label> ListLabelsTime { get; set; }
    public List<double> MarginsForLabelsTime { get; set; }

    public bool isViewEmty { get; set; }
    public bool isViewShadule { get; set; }
    public int Count_Notifications { get; set; }

    public ShaduleManager()
	{
		InitializeComponent();
        Current_shedule = null;
        Loaded += ShaduleManager_Loaded;
        ListLabelsTime = new List<Label>()
        {
            time_0_0,
            time_0_30,
            time_1_0,
            time_1_30,
            time_2_0,
            time_2_30,
            time_3_0,
            time_3_30,
            time_4_0,
            time_4_30,
            time_5_0,
            time_5_30,
            time_6_0,
            time_6_30,
            time_7_0,
            time_7_30,
            time_8_0,
            time_8_30,
            time_9_0,
            time_9_30,
            time_10_0,
            time_10_30,
            time_11_0,
            time_11_30,
            time_12_0,
            time_12_30,
            time_13_0,
            time_13_30,
            time_14_0,
            time_14_30,
            time_15_0,
            time_15_30,
            time_16_0,
            time_16_30,
            time_17_0,
            time_17_30,
            time_18_0,
            time_18_30,
            time_19_0,
            time_19_30,
            time_20_0,
            time_20_30,
            time_21_0,
            time_21_30,
            time_22_0,
            time_22_30,
            time_23_0,
            time_23_30,
            time_24_0
        };
        MarginsForLabelsTime = new List<double>();
        BindingContext = this;
    }

    private void ShaduleManager_Loaded(object? sender, EventArgs e)
    {
        if(StaticData.User.IsMaster)
        {
            Grid_MasterInfo.BindingContext = StaticData.User.Master;
        }

        SelectedDate = DatePicker_selected.Date;

        InitShadule(SelectedDate);
        InitBottomCalendar(SelectedDate);
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
        await Navigation.PopModalAsync(animated: false);
    }

    public async void Add_ShaduleCurrentDay(object sender, TappedEventArgs e)
    {
        if(Current_shedule == null)
        {
            PopUp_AddShaduleCurrentDay popUp_Add = new PopUp_AddShaduleCurrentDay(SelectedDate);
            popUp_Add.CreatedSheduleEvent += PopUp_Add_CreatedSheduleEvent;
            await Navigation.PushModalAsync(popUp_Add, false);
        }
        else
        {


        }
        
    }

    private void PopUp_Add_CreatedSheduleEvent(Model_schedule shedule)
    {
        Current_shedule = shedule;
        StaticData.User.Master.Schedules.Add(shedule);
        InitShadule(SelectedDate);
        InitBottomCalendar(SelectedDate);
    }

    public void InitTimeLabes(string from, string to)
    {
        var _startTime = from.Split(':');
        int startHours = Convert.ToInt32(_startTime[0]);
        int startMinutes = Convert.ToInt32(_startTime[1]);

        var _endTime = to.Split(':');
        int endHours = Convert.ToInt32(_endTime[0]);
        int endMinutes = Convert.ToInt32(_endTime[1]);

        int startIndex = startHours*2;
        int endIndex = endHours*2;
        if (startMinutes > 0)
        {
            startIndex++;
        }
        if (endMinutes > 0)
        {
            endIndex++;
        }
        MarginsForLabelsTime = new List<double>();
        double currentMargin = 30;
        for (int i = 0; i < ListLabelsTime.Count; i++)
        {
            if(i >= startIndex && i <= endIndex)
            {
                ListLabelsTime[i].IsVisible = true;
                MarginsForLabelsTime.Add(currentMargin);
                currentMargin += 54.33;
            }
            else
            {
                ListLabelsTime[i].IsVisible = false;
                MarginsForLabelsTime.Add(0);
            }
        }
    }

    public double GetCurrentMargin(string time)
    {
        var _startTime = time.Split(':');
        int startHours = Convert.ToInt32(_startTime[0]);
        int startMinutes = Convert.ToInt32(_startTime[1]);

        int startIndex = startHours * 2;
        if (startMinutes > 0)
        {
            startIndex++;
        }
        return MarginsForLabelsTime[startIndex];
    }

    public async Task InitBottomCalendar(DateTime date)
    {
        DayOfWeek Day = date.DayOfWeek;
        dateTimesWeeks = new List<ItemGridWeek>();
        switch (Day)
        {
            case DayOfWeek.Monday:
                {
                    //Понедельник
                    DateTime tempDate = date;
                    ChangeGridWeek(Grid_Mon, tempDate, "Mon");

                    //Вторник
                    tempDate = date.AddDays(1);
                    ChangeGridWeek(Grid_Tue, tempDate, "Tue");

                    //Среда
                    tempDate = date.AddDays(2);
                    ChangeGridWeek(Grid_Wed, tempDate, "Wed");

                    //Четверг
                    tempDate = date.AddDays(3);
                    ChangeGridWeek(Grid_Thu, tempDate, "Thu");

                    //Пятница
                    tempDate = date.AddDays(4);
                    ChangeGridWeek(Grid_Fri, tempDate, "Fri");

                    //Суббота
                    tempDate = date.AddDays(5);
                    ChangeGridWeek(Grid_Sat, tempDate, "Sat");

                    //Воскресенье
                    tempDate = date.AddDays(6);
                    ChangeGridWeek(Grid_Sun, tempDate, "Sun");

                    break;
                }
            case DayOfWeek.Tuesday:
                {
                    //Понедельник
                    DateTime tempDate = date.AddDays(-1);
                    ChangeGridWeek(Grid_Mon, tempDate, "Mon");

                    //Вторник
                    tempDate = date;
                    ChangeGridWeek(Grid_Tue, tempDate, "Tue");

                    //Среда
                    tempDate = date.AddDays(1);
                    ChangeGridWeek(Grid_Wed, tempDate, "Wed");

                    //Четверг
                    tempDate = date.AddDays(2);
                    ChangeGridWeek(Grid_Thu, tempDate, "Thu");

                    //Пятница
                    tempDate = date.AddDays(3);
                    ChangeGridWeek(Grid_Fri, tempDate, "Fri");

                    //Суббота
                    tempDate = date.AddDays(4);
                    ChangeGridWeek(Grid_Sat, tempDate, "Sat");

                    //Воскресенье
                    tempDate = date.AddDays(5);
                    ChangeGridWeek(Grid_Sun, tempDate, "Sun");

                    break;
                }
            case DayOfWeek.Wednesday:
                {
                    //Понедельник
                    DateTime tempDate = date.AddDays(-2);
                    ChangeGridWeek(Grid_Mon, tempDate, "Mon");

                    //Вторник
                    tempDate = date.AddDays(-1);
                    ChangeGridWeek(Grid_Tue, tempDate, "Tue");

                    //Среда
                    tempDate = date;
                    ChangeGridWeek(Grid_Wed, tempDate, "Wed");

                    //Четверг
                    tempDate = date.AddDays(1);
                    ChangeGridWeek(Grid_Thu, tempDate, "Thu");

                    //Пятница
                    tempDate = date.AddDays(2);
                    ChangeGridWeek(Grid_Fri, tempDate, "Fri");

                    //Суббота
                    tempDate = date.AddDays(3);
                    ChangeGridWeek(Grid_Sat, tempDate, "Sat");

                    //Воскресенье
                    tempDate = date.AddDays(4);
                    ChangeGridWeek(Grid_Sun, tempDate, "Sun");

                    break;
                }
            case DayOfWeek.Thursday:
                {
                    //Понедельник
                    DateTime tempDate = date.AddDays(-3);
                    ChangeGridWeek(Grid_Mon, tempDate, "Mon");

                    //Вторник
                    tempDate = date.AddDays(-2);
                    ChangeGridWeek(Grid_Tue, tempDate, "Tue");

                    //Среда
                    tempDate = date.AddDays(-1);
                    ChangeGridWeek(Grid_Wed, tempDate, "Wed");

                    //Четверг
                    tempDate = date;
                    ChangeGridWeek(Grid_Thu, tempDate, "Thu");

                    //Пятница
                    tempDate = date.AddDays(1);
                    ChangeGridWeek(Grid_Fri, tempDate, "Fri");

                    //Суббота
                    tempDate = date.AddDays(2);
                    ChangeGridWeek(Grid_Sat, tempDate, "Sat");

                    //Воскресенье
                    tempDate = date.AddDays(3);
                    ChangeGridWeek(Grid_Sun, tempDate, "Sun");

                    break;
                }
            case DayOfWeek.Friday:
                {
                    //Понедельник
                    DateTime tempDate = date.AddDays(-4);
                    ChangeGridWeek(Grid_Mon, tempDate, "Mon");

                    //Вторник
                    tempDate = date.AddDays(-3);
                    ChangeGridWeek(Grid_Tue, tempDate, "Tue");

                    //Среда
                    tempDate = date.AddDays(-2);
                    ChangeGridWeek(Grid_Wed, tempDate, "Wed");

                    //Четверг
                    tempDate = date.AddDays(-1);
                    ChangeGridWeek(Grid_Thu, tempDate, "Thu");

                    //Пятница
                    tempDate = date;
                    ChangeGridWeek(Grid_Fri, tempDate, "Fri");

                    //Суббота
                    tempDate = date.AddDays(1);
                    ChangeGridWeek(Grid_Sat, tempDate, "Sat");

                    //Воскресенье
                    tempDate = date.AddDays(2);
                    ChangeGridWeek(Grid_Sun, tempDate, "Sun");

                    break;
                }
            case DayOfWeek.Saturday:
                {
                    //Понедельник
                    DateTime tempDate = date.AddDays(-5);
                    ChangeGridWeek(Grid_Mon, tempDate, "Mon");

                    //Вторник
                    tempDate = date.AddDays(-4);
                    ChangeGridWeek(Grid_Tue, tempDate, "Tue");

                    //Среда
                    tempDate = date.AddDays(-3);
                    ChangeGridWeek(Grid_Wed, tempDate, "Wed");

                    //Четверг
                    tempDate = date.AddDays(-2);
                    ChangeGridWeek(Grid_Thu, tempDate, "Thu");

                    //Пятница
                    tempDate = date.AddDays(-1);
                    ChangeGridWeek(Grid_Fri, tempDate, "Fri");

                    //Суббота
                    tempDate = date;
                    ChangeGridWeek(Grid_Sat, tempDate, "Sat");

                    //Воскресенье
                    tempDate = date.AddDays(1);
                    ChangeGridWeek(Grid_Sun, tempDate, "Sun");

                    break;
                }
            case DayOfWeek.Sunday:
                {
                    //Понедельник
                    DateTime tempDate = date.AddDays(-6);
                    ChangeGridWeek(Grid_Mon, tempDate, "Mon");

                    //Вторник
                    tempDate = date.AddDays(-5);
                    ChangeGridWeek(Grid_Tue, tempDate, "Tue");

                    //Среда
                    tempDate = date.AddDays(-4);
                    ChangeGridWeek(Grid_Wed, tempDate, "Wed");

                    //Четверг
                    tempDate = date.AddDays(-3);
                    ChangeGridWeek(Grid_Thu, tempDate, "Thu");

                    //Пятница
                    tempDate = date.AddDays(-2);
                    ChangeGridWeek(Grid_Fri, tempDate, "Fri");

                    //Суббота
                    tempDate = date.AddDays(-1);
                    ChangeGridWeek(Grid_Sat, tempDate, "Sat");

                    //Воскресенье
                    tempDate = date;
                    ChangeGridWeek(Grid_Sun, tempDate, "Sun");

                    break;
                }
        }
    }

    public void GridWeek_Toch(object sender, TappedEventArgs e)
    {
        Grid sel_grid = (Grid)sender;
        string id = sel_grid.Id.ToString();
        for(int i = 0; i < dateTimesWeeks.Count; i++)
        {
            if (dateTimesWeeks[i].Id == id)
            {
                SelectedDate = dateTimesWeeks[i].Date;
                InitShadule(SelectedDate);
                InitBottomCalendar(SelectedDate);
                DatePicker_selected.Date = SelectedDate;
                break;
            }
        }
    }

    public void ChangeGridWeek(Grid gridWeek, DateTime date, string name)
    {
        Label dayWeek = (Label)gridWeek.Children[0];
        Label dateGrid = (Label)gridWeek.Children[1];

        ItemGridWeek itemGridWeek = new ItemGridWeek();
        itemGridWeek.Id = gridWeek.Id.ToString();
        itemGridWeek.Name = name;
        itemGridWeek.Date = date;
        itemGridWeek.ViewGrid = gridWeek;

        dateTimesWeeks.Add(itemGridWeek);
        if (SelectedDate.Day == date.Day && SelectedDate.Month == date.Month && SelectedDate.Year == date.Year)
        {
            gridWeek.BackgroundColor = Color.FromRgba("#8f97ec");
            dayWeek.TextColor = Colors.White;
            dateGrid.TextColor = Colors.White;
        }
        else if(DateTime.Now.Day == date.Day && DateTime.Now.Month == date.Month && DateTime.Now.Year == date.Year)
        {
            gridWeek.BackgroundColor = Colors.Transparent;
            dayWeek.TextColor = Color.FromRgba("#8f97ec");
            dateGrid.TextColor = Color.FromRgba("#8f97ec");
        }
        else if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            gridWeek.BackgroundColor = Colors.Transparent;
            dayWeek.TextColor = Color.FromRgba("#e6a6be");
            dateGrid.TextColor = Color.FromRgba("#e6a6be");
        }
        else
        {
            gridWeek.BackgroundColor = Colors.Transparent;
            dayWeek.TextColor = Colors.Gray;
            dateGrid.TextColor = Colors.Black;
        }
        dateGrid.Text = date.Day.ToString();
    }

    public async Task InitShadule(DateTime date)
    {
        if(StaticData.User.Master != null)
        {
            var result = await StaticData.API.POST("/get_schedule_byDay", new Dictionary<string, string>()
            {
                { "masterId", StaticData.User.Master._id },
                { "date", date.ToString()}
            });
            if (result != "")
            {
                ViewEmty.IsVisible = false;
                ViewShadule.IsVisible = true;

                List<Dictionary<string, string>> parsing = StaticData.API.ParsingListData(result);
                Console.WriteLine();
                for (int i = 0; i < parsing.Count; i++)
                {
                    var item = parsing[i];
                    Model_schedule schedule = new Model_schedule();
                    schedule.Date = StaticData.API.GetDateFromString(item["date"]);
                    schedule.SetDiaposonTimes(item["fromTime"], item["toTime"], 30);

                    Current_shedule = schedule;

                    InitTimeLabes(item["fromTime"], item["toTime"]);
                }
                //Сетка
                //for(int i = 0; i < MarginsForLabelsTime.Count; i++)
                //{
                //    var v = MarginsForLabelsTime[i];
                //    if(v != 0)
                //    {
                //        BoxView box = new BoxView();
                //        box.HorizontalOptions = LayoutOptions.Fill;
                //        box.VerticalOptions = LayoutOptions.Start;
                //        box.HeightRequest = 1;
                //        box.Margin = new Thickness(0,v,0,0);
                //        box.BackgroundColor = Colors.Black;
                //        Grid_BlockShadule.Add(box);
                //    }
                //}
                Grid_BlockShadule.Clear();
                for (int i = 0; i < StaticData.User.Master.Appointment_dates.Count; i++)
                {
                    var Appointment = StaticData.User.Master.Appointment_dates[i];

                    if(date.Day == Appointment.Date.Day &&
                       date.Month == Appointment.Date.Month &&
                       date.Year == Appointment.Date.Year)
                    {
                        Grid_BlockShadule.Add(InitBlockShadule(Appointment));
                    }
                }

                
            }
            else
            {
                ViewEmty.IsVisible = true;
                ViewShadule.IsVisible = false;
            }
        }
        
    }

    public Frame InitBlockShadule(Model_appointment_date Appointment)
    {
        Frame frame = new Frame();
        frame.BackgroundColor = Color.FromArgb("#f2e9fe");
        //top значение в зависимости от начала времени

        string time = StaticData.GetTime(Appointment.Date);
        double top_margin = GetCurrentMargin(time);

        frame.Margin = new Thickness(10, top_margin, 10, 0);
        frame.BorderColor = Colors.Transparent;
        frame.Padding = new Thickness(0);
        
        frame.HeightRequest = Appointment.GetTotalTime()*2; // Тоже зависит до которого времени запись
        frame.HorizontalOptions = LayoutOptions.Fill;
        frame.VerticalOptions = LayoutOptions.Start;

        Grid grid = new Grid();
        grid.HorizontalOptions = LayoutOptions.Fill;
        grid.VerticalOptions = LayoutOptions.Fill;

        BoxView boxView = new BoxView();
        boxView.BackgroundColor = Color.FromArgb("#dfccff");
        boxView.HorizontalOptions = LayoutOptions.Fill;
        boxView.VerticalOptions = LayoutOptions.Start;
        boxView.HeightRequest = 100;

        grid.Add(boxView);

        Label label = new Label();

        DateTime time1 = new DateTime();
        time1 = Appointment.Date;

        DateTime time2 = time1.AddMinutes(Appointment.GetTotalTime());

        label.Text = StaticData.GetTime(time1) + " - " + StaticData.GetTime(time2);
        label.TextColor = Colors.Black;
        label.Margin = new Thickness(10, 5, 0, 0);

        grid.Add(label);

        Image image = new Image();
        image.Source = "icon_new.png";
        image.Margin = new Thickness(0, -20, 0, 0);
        image.HeightRequest = 70;
        image.WidthRequest = 70;
        image.HorizontalOptions = LayoutOptions.End;
        image.VerticalOptions = LayoutOptions.Start;
        image.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#8e79b2") });

        grid.Add(image);

        ScrollView scrollView = new ScrollView();
        scrollView.Margin = new Thickness(0, 30, 0, 0);
        scrollView.HorizontalOptions = LayoutOptions.Fill;
        scrollView.VerticalOptions = LayoutOptions.Fill;

        VerticalStackLayout stack = new VerticalStackLayout();
        stack.Margin = new Thickness(10, 0, 0, 0);

        Label nameUser = new Label();
        nameUser.Text = Appointment.User.Name;
        nameUser.TextColor = Colors.Black;
        nameUser.FontFamily = "OpenSans-Semibold.ttf";

        stack.Add(nameUser);

        for(int i = 0; i < Appointment.Catergory_Item_Sevice.Count; i++)
        {
            var item = Appointment.Catergory_Item_Sevice[i];
            Label service = new Label();
            service.Text = item.Name + " - " + item.Price + " zł";
            service.FontSize = 13;
            service.TextColor = Colors.Black;

            stack.Add(service);
        }
        

        scrollView.Content = stack;

        grid.Add(scrollView);

        frame.Content = grid;

        return frame;
    }

    private void DatePicker_selected_DateSelected(object sender, DateChangedEventArgs e)
    {
        SelectedDate = e.NewDate;
        InitShadule(SelectedDate);
        InitBottomCalendar(SelectedDate);
    }
}