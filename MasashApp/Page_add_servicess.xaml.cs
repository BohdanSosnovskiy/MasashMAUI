using MasashApp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace MasashApp;

public partial class Page_add_servicess : ContentPage
{
	public Page_add_servicess()
	{
		InitializeComponent();
        StaticData.selectedCategoryService = null;
        Loaded += Page_add_servicess_Loaded;
	}

    private async void Page_add_servicess_Loaded(object? sender, EventArgs e)
    {
        Loaded -= Page_add_servicess_Loaded;

        //StaticData.User.Master.Catergory_Sevice = new ObservableCollection<Model_service_catogory>();
        //StaticData.User.Master.SelectedCatergory_Item_Sevice = new ObservableCollection<Model_service_item>();

        await StaticData.linkMainPage.loadData.LoadDataCategoryServices(StaticData.User.Master);

        

        if(StaticData.User.Master.Catergory_Sevice.Count > 0)
        {
            ListView_Categorys.BindingContext = StaticData.User.Master.Catergory_Sevice;
            ListView_Categorys.ItemsSource = StaticData.User.Master.Catergory_Sevice;
        }
        else
        {
            ListView_Categorys.BindingContext = new ObservableCollection<Model_service_catogory>();
            ListView_Categorys.ItemsSource = new ObservableCollection<Model_service_catogory>();
        }

        ListView_Categorys.ItemTapped += ListView_Categorys_ItemTapped;

        if (StaticData.User.Master.Catergory_Sevice.Count > 0)
        {
            if(StaticData.User.Master.Catergory_Sevice[0].Items.Count > 0)
            {
                ListView_CategorysItems.BindingContext = StaticData.User.Master.Catergory_Sevice[0].Items;
                ListView_CategorysItems.ItemsSource = StaticData.User.Master.Catergory_Sevice[0].Items;
            }
            else
            {
                ListView_CategorysItems.BindingContext = new ObservableCollection<Model_service_item>();
                ListView_CategorysItems.ItemsSource = new ObservableCollection<Model_service_item>();
            }
        }
    }

    public async void Add_category(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new PopUp_add_category_service(), false);
    }

    public async void Edit_category(object sender, TappedEventArgs e)
    {
        if (StaticData.selectedCategoryService != null)
        {
            await Navigation.PushModalAsync(new PopUp_edit_category_service(), false);
        }
        else
        {
            var alert = new ModalMessage()
            {
                IsButtonVisible = false,
                Text_yes = "Удалить",
                Text_no = "Отмена",
                Title_Message = "Ошибка",
                Message = "Выбирите категорию которую хотите изменить"
            };
            alert.ResponceUserEvent += Alert_RemoveCaterory;
            await Navigation.PushModalAsync(alert, false);
        }
    }

    public async void Remove_category(object sender, TappedEventArgs e)
    {
        var alert = new ModalMessage()
        {
            IsButtonVisible = true,
            Text_yes = "Удалить",
            Text_no = "Отмена",
            Title_Message = "Удалить категорию?",
            Message = "Вы действительно хотите удалить категорию? все услуги связаные с ней будут так же удалены"
        };
        alert.ResponceUserEvent += Alert_RemoveCaterory;
        await Navigation.PushModalAsync(alert, false);
    }

    private async void Alert_RemoveCaterory(string message)
    {
        if(message == "Yes")
        {
            var remove_cat = await StaticData.API.POST("/remove_category_service", new Dictionary<string, string>()
            {
                { "categoryId", selected_category._id}
            });

            await StaticData.linkMainPage.loadData.LoadDataCategoryServices(StaticData.User.Master);

            UpdateCollection();
        }
    }

    public void UpdateCollection()
    {
        if (StaticData.User.Master.Catergory_Sevice.Count > 0)
        {
            ListView_Categorys.BindingContext = StaticData.User.Master.Catergory_Sevice;
            ListView_Categorys.ItemsSource = StaticData.User.Master.Catergory_Sevice;
        }
        else
        {
            ListView_Categorys.BindingContext = new ObservableCollection<Model_service_catogory>();
            ListView_Categorys.ItemsSource = new ObservableCollection<Model_service_catogory>();
        }
        if (StaticData.User.Master.Catergory_Sevice.Count > 0)
        {
            if (StaticData.User.Master.Catergory_Sevice[0].Items.Count > 0)
            {
                ListView_CategorysItems.BindingContext = StaticData.User.Master.Catergory_Sevice[0].Items;
                ListView_CategorysItems.ItemsSource = StaticData.User.Master.Catergory_Sevice[0].Items;
            }
            else
            {
                ListView_CategorysItems.BindingContext = new ObservableCollection<Model_service_item>();
                ListView_CategorysItems.ItemsSource = new ObservableCollection<Model_service_item>();
            }
        }
    }

    public async void Add_service(object sender, TappedEventArgs e)
    {
        if(StaticData.selectedCategoryService != null)
        {
            await Navigation.PushModalAsync(new PopUp_add_service(), false);
        }
        else
        {
            var alert = new ModalMessage()
            {
                IsButtonVisible = false,
                Text_yes = "Удалить",
                Text_no = "Отмена",
                Title_Message = "Ошибка",
                Message = "Для начала выбирите категорию для которой хотите создать услугу"
            };
            await Navigation.PushModalAsync(alert, false);
        }
    }

    public async void Edit_service(object sender, TappedEventArgs e)
    {
        if(SelectedCatergory_Item_Sevice.Count > 0)
        {
            if(SelectedCatergory_Item_Sevice.Count == 1)
            {
                await Navigation.PushModalAsync(new PopUp_edit_services(SelectedCatergory_Item_Sevice[0]), false);
            }
            else
            {
                var alert = new ModalMessage()
                {
                    IsButtonVisible = false,
                    Text_yes = "Удалить",
                    Text_no = "Отмена",
                    Title_Message = "Ошибка",
                    Message = "Редактирование возможно только одной услуги"
                };
                await Navigation.PushModalAsync(alert, false);
            }
        }
        else
        {
            var alert = new ModalMessage()
            {
                IsButtonVisible = false,
                Text_yes = "Удалить",
                Text_no = "Отмена",
                Title_Message = "Ошибка",
                Message = "Для начала выбирите услугу"
            };
            await Navigation.PushModalAsync(alert, false);
        }
    }

    public async void Remove_service(object sender, TappedEventArgs e)
    {
        if (SelectedCatergory_Item_Sevice.Count > 0)
        {
            var alert = new ModalMessage()
            {
                IsButtonVisible = true,
                Text_yes = "Удалить",
                Text_no = "Отмена",
                Title_Message = "Удалить категорию?",
                Message = "Вы действительно хотите удалить категорию? все услуги связаные с ней будут так же удалены"
            };
            alert.ResponceUserEvent += Alert_RemoveItems;
            await Navigation.PushModalAsync(alert, false);
        }
        else
        {
            var alert = new ModalMessage()
            {
                IsButtonVisible = false,
                Text_yes = "Удалить",
                Text_no = "Отмена",
                Title_Message = "Ошибка",
                Message = "Для начала выбирите услуги (возможно несколько)"
            };
            await Navigation.PushModalAsync(alert, false);
        }
    }

    private async void Alert_RemoveItems(string message)
    {
        if (message == "Yes")
        {
            string massId = "";
            for(int i = 0; i < SelectedCatergory_Item_Sevice.Count; i++)
            {
                massId += SelectedCatergory_Item_Sevice[i]._id + ",";
            }
            var remove_cat = await StaticData.API.POST("/remove_service", new Dictionary<string, string>()
            {
                { "servisesId", massId}
            });

            await StaticData.linkMainPage.loadData.LoadDataCategoryServices(StaticData.User.Master);

            UpdateCollection();
        }
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

    Model_service_catogory selected_category = new Model_service_catogory();

    private void ListView_Categorys_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
        selected_category = (Model_service_catogory)e.Item;
        StaticData.selectedCategoryService = selected_category;
        ObservableCollection<Model_service_item> Items = selected_category.Items;
        if(Items.Count > 0)
        {
            ListView_CategorysItems.BindingContext = Items;
            ListView_CategorysItems.ItemsSource = Items;
        }
        else
        {
            ListView_CategorysItems.BindingContext = new ObservableCollection<Model_service_item>();
            ListView_CategorysItems.ItemsSource = new ObservableCollection<Model_service_item>();
        }
    }

    //Список выбранных элементов
    public ObservableCollection<Model_service_item> SelectedCatergory_Item_Sevice = new ObservableCollection<Model_service_item>();

    private void ListView_CategorysItems_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Model_service_item? item = e.Item as Model_service_item;
        if (item != null)
        {
            if (item.IsSelected)
            {
                for (int i = 0; i < SelectedCatergory_Item_Sevice.Count; i++)
                {
                    if (SelectedCatergory_Item_Sevice[i].Name == item.Name)
                    {
                        SelectedCatergory_Item_Sevice.RemoveAt(i);
                        break;
                    }
                }
                item.IsSelected = false;
            }
            else
            {
                item.IsSelected = true;
                SelectedCatergory_Item_Sevice.Add(item);
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
        }
    }
}