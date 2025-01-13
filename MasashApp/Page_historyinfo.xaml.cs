using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core.Views;
using MasashApp.Models;
using Microsoft.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MasashApp;

public partial class Page_historyinfo : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Page_historyinfo()
	{
		InitializeComponent();

        Loaded += Page_historyinfo_Loaded;
	}

    public void NextAppointment_Toch(object sender, TappedEventArgs e)
    {
        InitTopMenu(Appointment_datesNext, 1);
    }

    public void PrewAppointment_Toch(object sender, TappedEventArgs e)
    {
        InitTopMenu(Appointment_datesPrew, 2);
    }

    public void CancelAppointment_Toch(object sender, TappedEventArgs e)
    {
        InitTopMenu(new ObservableCollection<Model_appointment_date>(), 3);
    }

    public void InitTopMenu(ObservableCollection<Model_appointment_date> list, int variant)
    {
        Label nextLabel = (Label)NextAppointment.Children[0];
        BoxView nextBoxView = (BoxView)NextAppointment.Children[1];

        nextLabel.Text = $"Будущие ({Appointment_datesNext.Count})";

        Label prewLabel = (Label)PrewAppointment.Children[0];
        BoxView prewBoxView = (BoxView)PrewAppointment.Children[1];

        prewLabel.Text = $"Предыдущие ({Appointment_datesPrew.Count})";

        Label cancelLabel = (Label)CancelAppointment.Children[0];
        BoxView cancelBoxView = (BoxView)CancelAppointment.Children[1];

        cancelLabel.Text = $"Отмененые (0)";

        switch (variant)
        {
            case 1:
                {
                    nextBoxView.BackgroundColor = Colors.Green;
                    prewBoxView.BackgroundColor = Colors.Gray;
                    cancelBoxView.BackgroundColor= Colors.Gray;
                    break;
                }
            case 2:
                {
                    nextBoxView.BackgroundColor = Colors.Gray;
                    prewBoxView.BackgroundColor = Colors.Green;
                    cancelBoxView.BackgroundColor = Colors.Gray;
                    break;
                }
            case 3:
                {
                    nextBoxView.BackgroundColor = Colors.Gray;
                    prewBoxView.BackgroundColor = Colors.Gray;
                    cancelBoxView.BackgroundColor = Colors.Green;
                    break;
                }
        }

        InitListAppointment(list);
    }

    private void Page_historyinfo_Loaded(object? sender, EventArgs e)
    {
        Appointment_datesNext = new ObservableCollection<Model_appointment_date>();
        Appointment_datesPrew = new ObservableCollection<Model_appointment_date>();

        for (int i = 0; i < StaticData.User.Appointment_dates.Count; i++)
        {
            var appointment = StaticData.User.Appointment_dates[i].Date;
            if (DateTime.Now <= appointment)
            {
                Appointment_datesNext.Add(StaticData.User.Appointment_dates[i]);
            }
            else
            {
                Appointment_datesPrew.Add(StaticData.User.Appointment_dates[i]);
            }
        }
        InitTopMenu(Appointment_datesNext, 1);
    }

    public void InitListAppointment(ObservableCollection<Model_appointment_date> list)
    {
        Stack_Appointment.Children.Clear();
        Appointment_dateDictionary = new Dictionary<string, Model_appointment_date>();
        if (list.Count >0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Frame frame = InitBlockAppointment(list[i]);
                Appointment_dateDictionary.Add(frame.Id.ToString(), list[i]);
                Stack_Appointment.Add(frame);
            }
        }
        else
        {

        }
    }
    public Model_appointment_date GetSelectedAppoitment(object sender)
    {
        Image img = (Image)sender;
        Frame frame = (Frame)img.Parent.Parent.Parent;
        string id = frame.Id.ToString();

        var keys = Appointment_dateDictionary.Keys;
        foreach (var key in keys)
        {
            if (key == id)
            {
                return Appointment_dateDictionary[key];
            }
        }
        return null;
    }

    public async void BlockAppointment_Delete_Touch(object sender, TappedEventArgs e)
    {
        Model_appointment_date Appoitment = GetSelectedAppoitment(sender);
        if(Appoitment != null)
        {
            var alert = new ModalMessage()
            {
                IsButtonVisible = true,
                Text_yes = "Удалить",
                Text_no = "Отмена",
                Title_Message = "Удалить категорию?",
                Message = "Вы действительно хотите удалить категорию? все услуги связаные с ней будут так же удалены"
            };
            alert.ResponceUserEvent += Alert_Delete_Appointment;
            await Navigation.PushModalAsync(alert, false);
        }
    }

    private async void Alert_Delete_Appointment(string message)
    {
        if (message == "Yes")
        {
            
        }
    }

    public async void BlockAppointment_Star_Touch(object sender, TappedEventArgs e)
    {
        Model_appointment_date Appoitment = GetSelectedAppoitment(sender);
        if (Appoitment != null)
        {
            await Navigation.PushModalAsync(new PopUp_AddReview(Appoitment), false);
        }
    }

    public void BlockAppointment_Comment_Touch(object sender, TappedEventArgs e)
    {
        Model_appointment_date Appoitment = GetSelectedAppoitment(sender);
        if (Appoitment != null)
        {

        }
    }

    public Frame InitBlockAppointment(Model_appointment_date Appointment)
    {
        Frame frame = new Frame();
        frame.BackgroundColor = Color.FromArgb("#f2e9fe");
        
        string time = StaticData.GetTime(Appointment.Date);

        frame.Margin = new Thickness(10, 10, 10, 0);
        frame.BorderColor = Colors.Transparent;
        frame.Padding = new Thickness(0);

        frame.HeightRequest = 180;
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

        label.Text = $"{Appointment.Date.Day}.{Appointment.Date.Month}.{Appointment.Date.Year} {StaticData.GetTime(time1)} - {StaticData.GetTime(time2)}";
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
        nameUser.Text = "Мастер: " + Appointment.Master.Name;
        nameUser.TextColor = Colors.Black;
        nameUser.FontFamily = "OpenSans-Semibold.ttf";

        stack.Add(nameUser);

        for (int i = 0; i < Appointment.Catergory_Item_Sevice.Count; i++)
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

        Grid bottomGrid = new Grid();
        bottomGrid.HorizontalOptions = LayoutOptions.Fill;
        bottomGrid.VerticalOptions = LayoutOptions.End;
        bottomGrid.Margin = new Thickness(-1, 0, -1, -1);
        bottomGrid.HeightRequest = 50;
        bottomGrid.BackgroundColor = Color.FromArgb("#9c8fb2");
        bottomGrid.ColumnDefinitions.Add(new ColumnDefinition());
        bottomGrid.ColumnDefinitions.Add(new ColumnDefinition());
        bottomGrid.ColumnDefinitions.Add(new ColumnDefinition());

        Image img_comment = new Image();
        TapGestureRecognizer Comment_Touch = new TapGestureRecognizer();
        Comment_Touch.Tapped += BlockAppointment_Comment_Touch;
        img_comment.GestureRecognizers.Add(Comment_Touch);
        img_comment.Source = "comment.png";
        img_comment.HeightRequest = 70;
        img_comment.WidthRequest = 70;
        img_comment.HorizontalOptions = LayoutOptions.Center;
        img_comment.VerticalOptions = LayoutOptions.Center;
        img_comment.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#dfccff") });
        bottomGrid.Add(img_comment,0);

        Image img_star = new Image();
        TapGestureRecognizer Star_Touch = new TapGestureRecognizer();
        Star_Touch.Tapped += BlockAppointment_Star_Touch;
        img_star.GestureRecognizers.Add(Star_Touch);
        img_star.Source = "icon_star.png";
        img_star.HeightRequest = 70;
        img_star.WidthRequest = 70;
        img_star.HorizontalOptions = LayoutOptions.Center;
        img_star.VerticalOptions = LayoutOptions.Center;
        bool isFind = false;
        for (int i = 0; i < Appointment.Master.Reviews.Count; i++)
        {
            if (Appointment.Master.Reviews[i].Name == StaticData.User.Name && Appointment.Master.Reviews[i].AppointmentId == Appointment._id)
            {
                isFind = true;
                break;
            }
        }
        if(isFind)
        {
            img_star.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#fca103") });
        }
        else
        {
            img_star.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#dfccff") });
        }
        bottomGrid.Add(img_star, 1);

        Image img_delete = new Image();
        TapGestureRecognizer Delete_Touch = new TapGestureRecognizer();
        Delete_Touch.Tapped += BlockAppointment_Delete_Touch;
        img_delete.GestureRecognizers.Add(Delete_Touch);
        img_delete.Source = "trash.png";
        img_delete.HeightRequest = 70;
        img_delete.WidthRequest = 70;
        img_delete.HorizontalOptions = LayoutOptions.Center;
        img_delete.VerticalOptions = LayoutOptions.Center;
        img_delete.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.FromArgb("#dfccff") });
        bottomGrid.Add(img_delete, 2);

        grid.Add(bottomGrid);

        frame.Content = grid;

        return frame;
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

    private ObservableCollection<Model_appointment_date> appointment_datesNext;
    /// <summary>
    /// Список записей на прием которые будут
    /// </summary>
    public ObservableCollection<Model_appointment_date> Appointment_datesNext
    {
        get
        {
            return appointment_datesNext;
        }
        set
        {
            appointment_datesNext = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Model_appointment_date> appointment_datesPrew;
    /// <summary>
    /// Список записей на прием которые прошли
    /// </summary>
    public ObservableCollection<Model_appointment_date> Appointment_datesPrew
    {
        get
        {
            return appointment_datesPrew;
        }
        set
        {
            appointment_datesPrew = value;
            OnPropertyChanged();
        }
    }

    public Dictionary<string, Model_appointment_date> Appointment_dateDictionary { get; set; }
}