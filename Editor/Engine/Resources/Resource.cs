namespace LevelEditor.Engine.Resources
{
    public class Resource
    {
        private string name;
        protected bool ready;

        public string Name { get { return name; } }
        public bool Ready { get { return ready; } }

        public Resource(string name)
        {
            this.name = name;
            ready = false;
        }
    }
}
