#! /bin/sh
current_branch="$(git branch --show-current)"
echo "Checking JIRA reference for all non-merge-commits in branch: ${current_branch}"
# First, get all commit messages in this branch that are not in 'main' and exclude any merge-commits then remove blank lines.
# This gives us a nice, clean list of commit messages to work with.
# Normally grep will exit with 0 if ANY matches are found and 1 if no matches are found.
# The problem is we need to assert that ALL commit messages match.
# We can do this by asserting that the list of commit messages that don't start with a JIRA reference is empty.  
# We can do this by inverting the return value of the entire pipeline with ! at the beginning.
# Then, we simply output all lines that DONT start with a JIRA reference.
# If there are ANY lines in the output then the exit code will be 1 and this check will fail. 
# Otherwise if there were no commit messages that didn't start with a JIRA reference, 
# then that means that ALL commit messages DID start with a JIRA reference, the exit code will be 0 and the check passes.
# I love it when a good bash one-liner comes together.
! git log "${current_branch}" --not main --pretty=%B --no-merges | grep -v ^$ | grep -v "^\[[A-Z]*-[0-9]*\].*$"

