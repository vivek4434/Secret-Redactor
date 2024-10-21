# Secret-Redactor
Highly optimized library for sanitizing application logs with known set of secret.

Proposed algorithm make use of two-pointer technique to fetch all matching intervals for the given input string. Substring match is performed using polynomial hashing against two primes (namely p1, p2).
Hashes of all secret substring are pre-calculated to reduce the computation overhead (let's call it bag_of_hashes). After scanning input string, list of intervals (start_index, end_index) are generated (each interval represents a substring from input, whose hash can be found in bag_of_hashes).
All generated intervals are sorted by start_index, using this info interval merge is performed to get rid of overlapping intervals. Now its time to perform masking on actual input string, for that, string is scanned from start to end and as in when interval start is encountered, start is moved to current interval's end_index.
StringBuilder is used to create masked string to prevent unnecessary string allocations.
