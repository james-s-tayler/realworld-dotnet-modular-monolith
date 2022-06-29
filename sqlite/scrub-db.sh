#! /bin/bash

for db in `find -name "*.db"`
do
  echo "Scrubbing ${db}"
  echo "============================="
  for table in `sqlite3 ${db} "SELECT name FROM sqlite_schema WHERE type='table' AND name != 'VersionInfo';"`
  do
    echo "delete from ${table}"
    sqlite3 ${db} "delete from ${table}"
  done
  echo
done
