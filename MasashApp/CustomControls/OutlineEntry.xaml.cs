using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MasashApp.CustomControls;

public partial class OutlineEntry : Grid, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public OutlineEntry()
	{
		InitializeComponent();
        BindingContext = this;
	}

	public static readonly BindableProperty TextProperty = BindableProperty.Create(
			propertyName: nameof(Text),
			returnType: typeof(string),
			defaultValue: null,
			declaringType: typeof(OutlineEntry),
			defaultBindingMode: BindingMode.TwoWay
		);

	public string Text
	{
        get
        {
            return (string)GetValue(TextProperty);
        }
        set
        {
            SetValue(TextProperty, value);
            OnPropertyChanged();
        }
	}

    public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
            propertyName: nameof(Placeholder_custom),
            returnType: typeof(string),
            defaultValue: null,
            declaringType: typeof(OutlineEntry),
            defaultBindingMode: BindingMode.TwoWay
    );

    public string Placeholder_custom
    {
        get
        {
            string value = (string)GetValue(PlaceholderTextProperty);
            return value;
        }
        set
        {
            SetValue(PlaceholderTextProperty, value);
            OnPropertyChanged();
        }
    }

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		txtEntry.Focus();
    }

    private void txtEntry_Focused(object sender, FocusEventArgs e)
    {
        lblPlaceholder.IsVisible = false;
    }

    private void txtEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if(!string.IsNullOrWhiteSpace(Text))
        {
            lblPlaceholder.IsVisible = false;
        }
        else
        {
            lblPlaceholder.IsVisible = true;
        }
    }

    private void txtEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(Text))
        {
            lblPlaceholder.IsVisible = false;
        }
        else
        {
            lblPlaceholder.IsVisible = true;
        }
    }
}