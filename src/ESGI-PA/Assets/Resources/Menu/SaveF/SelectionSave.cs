[System.Serializable]
public class SelectionSave
{
    public int[] indices {get; private set;}
    public SelectionSave(int[] ins)
    {
        this.indices = ins;
    }
}
