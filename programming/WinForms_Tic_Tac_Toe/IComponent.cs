
namespace WinFormsApp1;

interface IComponent
{
    bool IsLocked { get; set; }
    void Enable();
    void Disable();
    void Reset();
}
