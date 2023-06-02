import os


def main(path: str = None, target_ext: list[str] = None):
    f = open("merged.cs", 'ab')

    target_dir = os.path.realpath(path if path is not None else os.path.join(__file__, ".."))
    target_ext = [x.replace('.', '') for x in target_ext]
    print(f"Counting lines for {target_ext} files in '{target_dir}'...")

    merge(f, target_dir, target_ext)
    f.close()


def merge(f, path: os.PathLike, target_ext: list[str] = None):
    for item_name in os.listdir(path):
        inod = os.path.join(path, item_name)
        if os.path.isfile(inod):
            if target_ext is not None and item_name.rsplit('.', 1)[-1] not in target_ext:
                continue
            if os.path.basename(path) == "merged.cs":
                continue

            merge_file(f, inod)
        else:
            merge(f, inod, target_ext)


def merge_file(f, path: os.PathLike) -> int:
    fname = os.path.basename(path)
    print(fname)
    with open(path, 'rb') as file:
        f.write(bytes(f"\n\n// {fname}\n\n", "utf-8"))
        f.write(file.read())


if __name__ == "__main__":
    main(target_ext=["cs"])
