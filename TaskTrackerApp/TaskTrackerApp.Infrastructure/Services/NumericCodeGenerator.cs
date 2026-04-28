using System.Security.Cryptography;
using TaskTrackerApp.Application.Interfaces;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class NumericCodeGenerator : INumericCodeGenerator
{
    public string Generate(int length)
    {
        int firstValue = (int)Math.Pow(10, length - 1);
        int maxValue = (int)Math.Pow(10, length);

        int randomNumber = RandomNumberGenerator.GetInt32(firstValue, maxValue);

        return randomNumber.ToString();
    }
}
