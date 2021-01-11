export function stringNotContainsOnlyNumbers(
  value: string | undefined
): boolean {
  if (value && value.match(/^[0-9]+$/) === null) {
    return true;
  } else {
    return false;
  }
}
