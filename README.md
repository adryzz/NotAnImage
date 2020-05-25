# NotAnImage
an RTLO (Right To Left Override) Attack demonstration
## What is RTLO?
This attack uses the unicode character U+202E to reverse all the text displayed on the screen, while, in the code the name remains normal.

Changing a file name with that character in it is the real problem.

For example, let's say you have enabled the option to see the file extensions in Windows. If i rename an executable file as `image\U202Egnp.exe`, you will see the name as `imageexe.png` and you will open it because it says `.png`.

So, if you set an icon and open the actual image that you have in a resource file into the executable, the user will never know that a malicious program is running in the background.

This is a demonstration, but **ONLY RUN IT IN A VM!** (I am not responsible for anything related to this application)

## What does this application do?
(If any of the actions failes, it will only show a BSoD)

1. Checks if it is already installed in the system.
2. If not, starts the setup
3. Suspends explorer.exe thread, so that the UI is unresponsive
4. Hides icons on the desktop
5. Copies itself (with the RTLO name) in any drive it has access to.
6. Setups itself at startup
7. Sets registry keys
8. Resumes explorer.exe
9. BSoD
10. When the pc starts up and the user logs in, every second it performs an action like minimizing a window or changing its title.
