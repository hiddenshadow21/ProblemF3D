# ProblemF3D
My solution to Problem F with 3D virtualization in WPF for ACM Central European Programming Contest, Warsaw 2001, Poland<br/>
I've modified the problem adding different shapes of cisterns. That's why i had to change the input so the first letter corresponds to a specific cistern and next values describes it's size.
# The problem
Problem F
Fill the cisterns!
ACM Central European Programming Contest, Warsaw 2001, Poland
During the next century certain regions on earth will experience severe water shortages. The old
town of Uqbar has already started to prepare itself for the worst. Recently they created a network of
pipes connecting the cisterns that distribute water in each neighbourhood, making it easier to fill them
at once from a single source of water. But in case of water shortage the cisterns above a certain level
will be empty since the water will flow to the cisterns below.
You have been asked to write a program to compute the level to which cisterns will be filled with a
certain volume of water, given the dimensions and position of each cistern. To simplify we will neglect
the volume of water in the pipes.
## Task
Write a program which for each data set:
* reads the description of cisterns and the volume of water,
* computes the level to which the cisterns will be filled with the given amount of water,
* writes the result.
## Input
The first line of the input contains the number of data sets k, 1 <= k <= 30. The data sets follow.
The first line of each data set contains one letter, idetifier of cistern, one integer n, the number of cisterns, 1 <= n <= 50 000. Each of
the following n lines consists of 4 nonnegative integers, separated by single spaces: b, h, w, d - the base
level of the cistern, its height, width and depth in meters, respectively. The integers satisfy 0 <= b <= 106
and 1 <= h <= w <= d <= 40 000. The last line of the data set contains an integer V - the volume of water in
cubic meters to be injected into the network. Integer V satisfies 1 <= V <= 2 <= 109.
## Output (console project)
The output should consist of exactly d lines, one line for each data set.
Line i, 1 <= i <= d, should contain the level that the water will reach, in meters, rounded up to two
fractional digits, or the word OVERFLOW, if the volume of water exceeds the total capacity of the cisterns.
## Output (WPF)
The output contains 3D view of cisterns filled with water. Double tap LMB to iterate between tasks and use RMB to change camera angle.
