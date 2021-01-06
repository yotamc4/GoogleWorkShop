import * as ControllerUtils from "./ControllerUtils";

import { GetBidsResponse } from "./BidsControllerService";

export async function GetBidsCreatedByBuyer(
  userId: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<GetBidsResponse> {
  const serviceUrl = BasicControllerUrl + "/api/v1/Buyers/" + userId;

  try {
    const response: Response = await ControllerUtils.makeGetRequest(
      serviceUrl,
      getAccessTokenSilently
    );

    const getBidsResponse: GetBidsResponse = (await response.json()) as GetBidsResponse;

    for (let i = 0; i < getBidsResponse.bidsPage.length; i++) {
      getBidsResponse.bidsPage[i].creationDate = new Date(
        getBidsResponse.bidsPage[i].creationDate
      );

      getBidsResponse.bidsPage[i].expirationDate = new Date(
        getBidsResponse.bidsPage[i].expirationDate!
      );
    }

    return getBidsResponse;
  } catch (e) {
    throw e;
  }
}

// TODO - need to update this function to the BE (they haven't implemented it)
export async function GetGroupsBuyerIsParticipant(
  userId: string
): Promise<GetBidsResponse> {
  const serviceUrl = BasicControllerUrl + "/api/v1/Buyers/" + userId;

  try {
    const response: Response = await ControllerUtils.makeGetRequest(serviceUrl);

    const getBidsResponse: GetBidsResponse = (await response.json()) as GetBidsResponse;

    for (let i = 0; i < getBidsResponse.bidsPage.length; i++) {
      getBidsResponse.bidsPage[i].creationDate = new Date(
        getBidsResponse.bidsPage[i].creationDate
      );

      getBidsResponse.bidsPage[i].expirationDate = new Date(
        getBidsResponse.bidsPage[i].expirationDate!
      );
    }

    return getBidsResponse;
  } catch (e) {
    throw e;
  }
}

const BasicControllerUrl: string = "https://localhost:5001";
