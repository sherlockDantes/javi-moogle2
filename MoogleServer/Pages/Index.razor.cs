using MoogleEngine;

namespace MoogleServer.Pages
{
    public partial class Index
    {

        private string query = "";
        private SearchResult result = new SearchResult();
        


        private void RunQuery()
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                result = Moogle.Query(query);


                
            }
        }
    }
}
