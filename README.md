twt
===

...is a **command line based [twitter](http://twitter.com/) client** written in .NET 4.0 using the [Twitterizer](http://www.twitterizer.net/) library and packaged into a single executable. It is OAuth compliant and pretty tiny. It is also [Growl for Windows](http://www.growlforwindows.com/gfw/) compliant and will integrate with the notification system.

twt actually only allows you to update your status from the command line, it cannot be used to browse Twitter.

Download
--------

I'll have a release up on Github soon.

Usage
-----

twt is simple!

### Configuration

Once twt is unpacked, you must allow it authorization to your twitter account. This can be done by typing `twt -config` at the command line. This will launch a browser window to the authorization page for twt. Once you allow access to twt, you will be given a PIN number to put into the input box that appears. When this is complete, you are ready to tweet from the command line.

There is also an option for "quiet mode" that suppresses all feedback from twt. Quiet mode can be turned on or off in the configuration:

`twt -set quiet <true|false>`

or as a command line switch:

`twt -q no messages popping up here!`

`twt -quiet if you want to be more verbose!`

### Tweeting

Tweeting is simple once twt is given authorization:

`twt I just got twt set up, it's pretty awesome!`

About the Author
----------------

twt was written by Nick Gordon. By using this software, you agree to never sue Nick Gordon for anything. Twt was migrated to a Github repo from GoogleCode in August 2018.