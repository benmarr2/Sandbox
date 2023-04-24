#!/bin/bash

# Function to convert a name to snake_case
to_snake_case() {
  echo "$1" | sed -e 's/ /_/g' -e 's/\([a-z0-9]\)\([A-Z]\)/\1_\2/g' | tr '[:upper:]' '[:lower:]'
}

# Function to rename all files and folders in a directory to snake_case
rename_items_in_directory() {
  for item in "$1"/*; do
    new_item_name="$(to_snake_case "$(basename "$item")")"
    new_item_path="$(dirname "$item")/$new_item_name"
    mv -n "$item" "$new_item_path"

    if [ -d "$new_item_path" ]; then
      rename_items_in_directory "$new_item_path"
    fi
  done
}

# Start renaming files and folders in the current folder and its subfolders
rename_items_in_directory "$(pwd)"

