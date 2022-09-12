import json
import subprocess
from typing import Optional


def get_version(build: Optional[str], version_type: str = "default") -> str:

    with open("version.json", "r") as fh:
        version_data = json.load(fh)

    # git commit hash
    commit = subprocess.check_output(["git", "rev-parse", "HEAD"], stdin=None, stderr=None, shell=False).decode("utf8").strip()

    # version
    version = version_data["version"]
    suffix = version_data["suffix"]

    if suffix:
        version = f"{version}-{suffix}"

        if build:

            # PEP440 does not support SemVer versioning (https://semver.org/#spec-item-9)
            if version_type == "pypi":
                version = f"{version}{int(build):03d}"

            else:
                version = f"{version}.{build}"

    if (version_type == "assembly"):
        version += "+" + commit

    return version
