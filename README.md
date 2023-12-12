Currently if you run the project the data will be read from the json file.
I have also written a simple project called PartnerXYZ_API which when run allows this project to make a call to that api to return the same sample data.
It just requires commenting out the "getfromfile" code in PartnerXYZ.cs method GetPartnerRatesAsync() and uncommenting the "getfromapi" code.
Then running the server before running this app.

The code is unfinished as expected, but I would like add a design doc to more fully communicate the intended design of the system. I do at least have written notes of the design.
I look forward to discussing it more in detail.
Thanks!
