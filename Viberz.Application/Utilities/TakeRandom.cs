namespace Viberz.Application.Utilities;

public class TakeRandom
{
    public static List<T> TakeRandomToList<T>(List<T> list, int numberOfElements, Random random)
    {
        List<T> copyList = [.. list];

        int n = copyList.Count;

        for (int i = 0; i < n; i++)
        {
            int j = random.Next(i, n);

            T temp = copyList[i];
            copyList[i] = copyList[j];
            copyList[j] = temp;
        }

        return copyList.Take(numberOfElements).ToList();
    }
}
