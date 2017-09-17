namespace RemoteControlService.Core
{
    public interface IApplication
    {
        void Start();
        void Stop();

        int GetCount();
    }
}
