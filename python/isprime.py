import math

def isPrime(number):
    if (not (number > 0)):
        print("Invalid number. Must be a positive number.")
        return False

    if (number % 1 != 0):
        print("Invalid number. Must be an integer.")
        return False

    if (number == 1):
        return False

    print("Testing if", number, "is a prime...")

    i = 2
    while (i <= math.ceil(number / 2)):
        if (number % i == 0):
            print("Found a factor: ", i)
            return False

        i = i+1

    return True

print("Result:", isPrime(47629))
