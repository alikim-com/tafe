
namespace WinFormsApp1;

internal interface IComponent
{
    void Enable();
    void Disable();
    void SimulateOnClick();
    string Name { get; }
}
