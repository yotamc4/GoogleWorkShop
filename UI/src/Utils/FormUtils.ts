export function stringNotContainsOnlyNumbers(
  value: string | undefined
): boolean {
  if (value && value.match(/^[0-9]+$/) === null) {
    return true;
  } else {
    return false;
  }
}

export const validateInputIsNumber = (value: string): string => {
  if (stringNotContainsOnlyNumbers(value)) {
    return "Only numbers allowed";
  } else {
    return "";
  }
};
