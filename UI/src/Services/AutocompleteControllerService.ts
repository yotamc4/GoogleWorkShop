import * as ControllerUtils from "./ControllerUtils";

export async function getAutoCompleteValues(): Promise<string[]> {
  try {
    const response: Response = await ControllerUtils.makeGetRequest(
      BasicControllerUrl + "/Products"
    );

    const autoCompleteValues: any = (await response.json()) as any;

    return autoCompleteValues as string[];
  } catch (e) {
    throw e;
  }
}

const BasicControllerUrl: string = `${process.env.REACT_APP_URL}/Autocomplete`;
