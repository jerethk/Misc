# Pig latin

# a function to get a sentence from the player, returns the sentence when a valid sentence is entered
def get_sentence():
    valid = False

    while (not valid):     # keep trying until a valid sentence has been entered
        user_input = input("\nPlease enter a sentence: ")

        if validate_string(user_input):
            valid = True
        else:
            print("Your sentence contained invalid characters. Please try again, using only letters and spaces.")

    return user_input

# A function to validate a string. It checks the string only has letters and spaces
def validate_string(sentence):
    for i in sentence:
        if (not i.isalpha()) and (i != " "):
            return False

    return True

# A function to check if there are no vowels in a word (A, E, I, O, U)
def no_vowels(char_list):
    # convert character list to set
    char_set = set()
    for i in char_list:
        char_set.add(i)

    # check for disjoint with set of vowels
    if char_set.isdisjoint({"a", "e", "i", "o", "u"}):
        return True
    else:
        return False

# Function for translating a word into Pig Latin
def translate_piglatin(word):
    # define set for vowel characters
    vowels = {"a", "e", "i", "o", "u"}

    # Create an array and transfer each character into it from the word, then append a hyphen
    character_array = list()
    for i in word:
        character_array.append(i)

    #character_array.append("-")

    # Determine if the first letter is a vowel
    if character_array[0] in vowels:
        vowel_word = True
    else:
        vowel_word = False

    # if there are no vowels in the word, treat the first "y" as a vowel
    if (no_vowels(character_array)):
        vowels.add("y")

    # Move characters from beginning to end of array until a vowel is reached
    for i in range(len(character_array)):
        if not (character_array[0] in vowels):
            character_array.append(character_array.pop(0))

    #while not (character_array[0] in vowels):
    #    character_array.append(character_array.pop(0))

    # Create a new string from the array of characters
    new_word = ""
    for i in character_array:
        new_word += i

    # Add the "ay" or "way" suffix, then return the translated word
    if vowel_word:
        new_word += "way"
    else:
        new_word += "ay"

    return new_word


# Code for the game itself
def play_game():
    # Gets a sentence from the player
    base_sentence = get_sentence()
    base_sentence = base_sentence.lower()

    # splits the sentence into words
    word_list = base_sentence.split()

    #string for translated sentence
    piglat_sentence = ""

    #translate each word into Pig Latin
    for i in word_list:
        piglat_word = translate_piglatin(i)
        piglat_sentence += piglat_word + " "

    #print the completed translated sentence
    print("\nYour sentence in Pig Latin is:")
    print(piglat_sentence)

# Program execution starts here -------------------------------------------------------------------
# Welcome message, allows player to enter their name and greets them
print("Welcome to the Pig Latin game!")
playername = input("Please enter your name: ")
print("Hello, " + playername + "!")

# Play the game at least once
play_game()

# Ask if play again?
while True:
    print("\nDo you want to play again?")
    answer = input("Enter ""Y"" to play again: ")

    if (answer == "Y" or answer == "y"):
        play_game()
    else:
        break

# Quit program
print("\nThank you for playing, " + playername + "! See you next time.")
print("Copyright J Kok 2021.")


