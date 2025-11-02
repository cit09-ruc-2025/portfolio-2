namespace WebServiceLayer.Models;

public class QueryParams
{
    private int pageSize = 25;
    private int page = 1;

    public int PageSize
    {
        get { return pageSize; }
        set { pageSize = value < 1 ? 1 : value; }
    }

    public int Page
    {
        get { return page; }
        set { page = value < 1 ? 1 : value; }
    }
}
