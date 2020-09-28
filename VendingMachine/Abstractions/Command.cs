namespace VendingMachine.Abstractions
{
    public abstract class Command
    {
        #region Properties
        protected IMachine Machine { get; }
        #endregion

        #region Constructors
        protected Command(IMachine machine)
        {
            Machine = machine;
        }
        #endregion

        #region Abstrcat Methods
        public abstract void Execute();
        #endregion

    }
}
