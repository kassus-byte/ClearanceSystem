namespace SchoolClearanceSystem.Repository
{
    public abstract class BaseRepository
    {

        // protected- only this class and children can see
        // readonly- can never be reassigned
        //abstract- exists to be inherited not used on its own
        protected readonly DatabaseManager dbManager = new DatabaseManager();
    }
}
    