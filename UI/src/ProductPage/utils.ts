export function getDate(expirationDate: string | Date | undefined) {
  return `${(new Date(expirationDate as string).getUTCMonth() as number) + 1}
    /
    ${(new Date(expirationDate as string).getUTCDate() as number) + 1}
    /
    ${new Date(expirationDate as string).getUTCFullYear() as number}`;
}
