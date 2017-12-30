namespace Travlendar.Core.AppCore.Model
{
    public class TicketModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Detail { get; set; }

        public override string ToString ()
        {
            return this.Image.ToString ();
        }
    }
}
