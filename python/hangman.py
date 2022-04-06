def hangman(word, chances):
    word = word.upper()         # change all letters to uppercase
    guessed_letters = set()     # set of previously guessed letters
    remaining_letters = set()   # set of remaining letters in word; removed as they are correctly guessed
    word_showing = list()       # correctly guessed characters to show

    for i in word:
        remaining_letters.add(i)
        word_showing.append("-")        # start with dashes

    print("Welcome to Hangman")
    print("The length of the word is", len(word), "characters.")

    while (chances > 0):
        showstring = ""
        for i in word_showing:
            showstring += i + " "
        print("\n" + showstring)

        print("You have", chances, "chances remaining")

        if len(guessed_letters) > 0:
            print("Letters already guessed", guessed_letters)

        guess = input("Enter a letter: ")

        if len(guess) != 1:
            print("Invalid entry, please try again")
            continue

        if guess.islower():
            guess = guess.upper()       # convert to uppercase

        if (guess in guessed_letters):
            print("You've already guessed that letter.")
            continue

        guessed_letters.add(guess)

        if (word).find(guess) == -1:
            print("Sorry, [", guess, "] is an incorrect guess.")
            chances = chances - 1
        else:
            print("Well done! [", guess, "] is a correct guess.")
            remaining_letters.remove(guess)
            for i in range(len(word)):
                if word[i] == guess:
                    word_showing[i] = guess     # reveal correctly guessed letter

        if len(remaining_letters) == 0:
            print("Congratulations! You've guessed all the letters!")
            print("The word was", word)
            break

    if chances == 0:
        print("You've run out of chances. You have died. Game over")


hangman("GuessThisWord", 5)