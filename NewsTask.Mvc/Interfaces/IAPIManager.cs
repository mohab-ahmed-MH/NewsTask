namespace NewsTask.Mvc.Interfaces
{
    public interface IAPIManager<T> where T : class
    {
        List<T> GetList(string controllerName);
        T GetById(int id, string controllerName);
        void CreateEntity(T entity, string controllerName);
        void UpdateEntity(T entity, string controllerName);
        void DeleteById(int id, string controllerName);


        public virtual string ControllerName
        {
            set { ControllerName = value; }
            get { return ControllerName; }
        }
    }
}
