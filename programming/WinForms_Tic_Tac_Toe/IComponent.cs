
namespace WinFormsApp1;

public interface IComponent
{
    bool IsLocked { get; set; }
    void Enable();
    void Disable();
}
