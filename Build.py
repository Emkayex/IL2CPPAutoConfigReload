from argparse import ArgumentParser
from datetime import datetime
from os import system as os_system
from pathlib import Path
from shutil import copy


def build(dst_folder: Path) -> None:
    BIN_PATH = Path(__file__).parent.joinpath("bin", "Debug", "net6.0")
    FILES_TO_BUNDLE = [
        BIN_PATH.joinpath("IL2CPPAutoConfigReload.dll"),
    ]
    dst_folder.mkdir(parents = True, exist_ok = True)

    os_system("dotnet clean")
    os_system("dotnet build")

    for src_path in FILES_TO_BUNDLE:
        dst_path = dst_folder.joinpath(src_path.name)
        copy(src_path, dst_path)

    print(datetime.now())


def main() -> None:
    parser = ArgumentParser()
    parser.add_argument("dst_folder", help = "The destination folder into which the built mod should be moved (for easy testing).")
    args = parser.parse_args()

    dst_folder = Path(args.dst_folder)
    build(dst_folder)


if __name__ == "__main__":
    main()
