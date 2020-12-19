export async function makePostRequest<T>(
  url: string,
  body: T
): Promise<Response> {
  const options = {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
    body: JSON.stringify(body),
  };

  return await fetch(url, options);
}
