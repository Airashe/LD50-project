namespace Airashe.UCore.Systems.Input
{
    public struct InputCommandStateChanged
    {
        public int CommandTypeIndex => commandTypeIndex;
        private int commandTypeIndex;

        public bool IsActive => isActive;
        private bool isActive;

        public InputCommandStateChanged(int commandTypeIndex, bool isActive)
        {
            this.commandTypeIndex = commandTypeIndex;
            this.isActive = isActive;
        }
    }
}
