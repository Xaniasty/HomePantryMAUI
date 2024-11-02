namespace HomePantry.Structure.Views;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage()
	{
		InitializeComponent();
		BindingContext = new CreateAccountViewModel();
	}
}


