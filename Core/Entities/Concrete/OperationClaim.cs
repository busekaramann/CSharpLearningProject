namespace Core.Entities.Concrete
{
    //Kullanıcıya verebileceğim rolleri verdiğim class 
    public class OperationClaim:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
