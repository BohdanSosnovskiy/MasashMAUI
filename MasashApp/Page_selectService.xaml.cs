using MasashApp.Models;
using System.Collections.ObjectModel;

namespace MasashApp;

public partial class Page_selectService : ContentPage
{
	public Page_selectService()
	{
		InitializeComponent();
        Loaded += Page_selectService_Loaded;
	}

    private void Page_selectService_Loaded(object? sender, EventArgs e)
    {
        Loaded -= Page_selectService_Loaded;

        StaticData.Catergory_Sevice = new System.Collections.ObjectModel.ObservableCollection<Model_service_catogory>();
        StaticData.SelectedCatergory_Item_Sevice = new System.Collections.ObjectModel.ObservableCollection<Model_service_item>();

        Model_service_catogory Category = new Model_service_catogory();
        Category.Name = "Маникюр";
        Category.PathImg = "manic_1.jpg";

        Model_service_item item = new Model_service_item();
        item.Name = "Манікюр гігієнічний ( без покриття)";
        item.PathImg = "manic_1.jpg";
        item.Price = 70;
        item.Time = 30;
        item.LinkModelCategory = Category;

        Category.Items.Add(item);
        item = new Model_service_item();
        item.Name = "Маникюр с покрытием";
        item.PathImg = "manic_2.jpg";
        item.Price = 140;
        item.Time = 60;
        item.LinkModelCategory = Category;

        Category.Items.Add(item);

        item = new Model_service_item();
        item.Name = "Гелевая коррекция (укрепление)";
        item.PathImg = "manic_4.jpg";
        item.Price = 160;
        item.Time = 60;
        item.LinkModelCategory = Category;

        Category.Items.Add(item);

        StaticData.Catergory_Sevice.Add(Category);

        Category = new Model_service_catogory();
        Category.Name = "Педикюр";
        Category.PathImg = "manic_3.jpg";

        item = new Model_service_item();
        item.Name = "Педикюр";
        item.PathImg = "manic_3.jpg";
        item.Price = 150;
        item.Time = 90;
        item.LinkModelCategory = Category;

        Category.Items.Add(item);

        StaticData.Catergory_Sevice.Add(Category);

        ListView_Categorys.BindingContext = StaticData.Catergory_Sevice;
        ListView_Categorys.ItemsSource = StaticData.Catergory_Sevice;
        ListView_Categorys.ItemTapped += ListView_Categorys_ItemTapped;

        ListView_CategorysItems.BindingContext = StaticData.Catergory_Sevice[0].Items;
        ListView_CategorysItems.ItemsSource = StaticData.Catergory_Sevice[0].Items;

        Carusel_Services.BindingContext = StaticData.SelectedCatergory_Item_Sevice;

        Carusel_Services.ItemsSource = StaticData.SelectedCatergory_Item_Sevice;

        CheckSelServises();
    }

    private void ListView_Categorys_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
        Model_service_catogory category = (Model_service_catogory)e.Item;
        ObservableCollection<Model_service_item> Items = category.Items;
        ListView_CategorysItems.BindingContext = Items;
        ListView_CategorysItems.ItemsSource = Items;
    }

    public void Back_Touch(object sender, TappedEventArgs e)
    {
        StaticData.linkMainPage.IsSelectedServise = false;
        StaticData.linkMainPage.IsNullServise = true;
        StaticData.SelectedCatergory_Item_Sevice.Clear();
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

    private void ListView_CategorysItems_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Model_service_item? item = e.Item as Model_service_item;
        if (item != null)
        {
            if(item.IsSelected)
            {
                for(int i = 0; i < StaticData.SelectedCatergory_Item_Sevice.Count; i++)
                {
                    if (StaticData.SelectedCatergory_Item_Sevice[i].Name == item.Name)
                    {
                        StaticData.SelectedCatergory_Item_Sevice.RemoveAt(i);
                        break;
                    }
                }
                item.IsSelected = false;
            }
            else
            {
                item.IsSelected = true;
                StaticData.SelectedCatergory_Item_Sevice.Add(item);
            }
            int selected = item.LinkModelCategory.CheckCountSelected();
            if (selected > 0)
            {
                item.LinkModelCategory.CountSelected = selected;
                item.LinkModelCategory.IsViewSelected = true;
            }
            else
            {
                item.LinkModelCategory.IsViewSelected = false;
            }

            CheckSelServises();
        }
    }

    public void CheckSelServises()
    {
        int cash = 0;

        if(StaticData.SelectedCatergory_Item_Sevice.Count > 0)
        {
            var result = StaticData.linkMainPage.CheckTotalTimeCash();
            cash = result["Cash"];

            Grid_SelServises.IsVisible = true;
            Text_SelServises.Text = $"Выбраные услуги: {StaticData.SelectedCatergory_Item_Sevice.Count} / {cash} zł";
           
        }
        else
        {
            Grid_SelServises.IsVisible = false;
        }
    }

    public void SelectServises(object sender, TappedEventArgs e)
    {
        if(StaticData.SelectedCatergory_Item_Sevice.Count > 0)
        {
            StaticData.linkMainPage.IsSelectedServise = true;
            StaticData.linkMainPage.IsNullServise = false;
        }
        else
        {
            StaticData.linkMainPage.IsSelectedServise = false;
            StaticData.linkMainPage.IsNullServise = true;
        }
        Close();
    }
}