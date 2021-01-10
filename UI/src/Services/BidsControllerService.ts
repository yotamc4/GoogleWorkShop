import { Bid, NewBidRequest } from "../Modal/GroupDetails";
import {
  BidBuyerJoinRequest,
  BidBuyerJoinRequest2,
} from "../Modal/ProductDetails";
import { IMarkPaidRequest } from "../PaymentTable/PaymentTable.interface";
import { ISupplierProposalRequest } from "../ProductPage/Suppliers/SupplierSection.interface";
import { IVotingRequest } from "../ProductPage/Suppliers/SupplierSurvey.interface";
import {
  buildUrlWithQueryParams,
  makeDeleteRequest,
  makeGetRequest,
  makeGetRequestAsync,
  makePostRequest,
} from "./ControllerUtils";

export async function submitNewGroupForm(
  bidRequest: NewBidRequest,
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<void> {
  const serviceUrl = BasicControllerUrl;

  const response: Response = await makePostRequest(
    serviceUrl,
    bidRequest,
    getAccessTokenSilently
  );

  if (!response.ok) {
    throw new Error("Error happened during the fetch POST");
  }
}

export async function getBids(
  intervalNumber: number,
  category: string | null,
  subCategory: string | null,
  searchString: string | null
): Promise<GetBidsResponse> {
  const serviceUrl = BasicControllerUrl;
  const queryParm: Map<string, string> = new Map([
    ["page", intervalNumber.toString()],
  ]);

  if (category) {
    queryParm.set("category", category);
  }

  if (subCategory) {
    queryParm.set("subCategory", subCategory);
  }

  if (searchString) {
    queryParm.set("search", searchString);
  }

  const options = {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
  };

  try {
    const response: Response = await fetch(
      buildUrlWithQueryParams(serviceUrl, queryParm),
      options
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

export function getBidSpecific(
  url: string,
  isAuthenticated: boolean,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  if (isAuthenticated) {
    return makeGetRequestAsync(serviceUrl, getAccessTokenSilently);
  } else {
    return makeGetRequestAsync(serviceUrl);
  }
}

export function getProposals(
  url: string,
  isAuthenticated: boolean,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  if (isAuthenticated) {
    return makeGetRequestAsync(serviceUrl, getAccessTokenSilently);
  } else {
    return makeGetRequestAsync(serviceUrl);
  }
}

export function getBidParticipations(
  url: string,
  isAuthenticated: boolean,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  if (isAuthenticated) {
    return makeGetRequest(serviceUrl, getAccessTokenSilently);
  } else {
    return makeGetRequest(serviceUrl);
  }
}

export function getBidParticipationsFullDetails(
  url: string,
  isAuthenticated: boolean,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  if (isAuthenticated) {
    return makeGetRequest(serviceUrl, getAccessTokenSilently);
  } else {
    return makeGetRequest(serviceUrl);
  }
}

export function addBuyer(
  bidBuyerJoinRequest: BidBuyerJoinRequest2,
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makePostRequest(
    serviceUrl,
    bidBuyerJoinRequest,
    getAccessTokenSilently
  );
  return response;
}

export function deleteBuyer(
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makeDeleteRequest(serviceUrl, "", getAccessTokenSilently);
  return response;
}

export function addSupplierProposal(
  supplierProposalFormDetails: Partial<ISupplierProposalRequest>,
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makePostRequest(
    serviceUrl,
    supplierProposalFormDetails,
    getAccessTokenSilently
  );
  return response;
}

export async function markPayment(
  markPaidRequest: IMarkPaidRequest,
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = await makePostRequest(
    serviceUrl,
    markPaidRequest,
    getAccessTokenSilently
  );
  return response;
}

export function deleteSupplierProposal(
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makeDeleteRequest(serviceUrl, "", getAccessTokenSilently);
  return response;
}

export function voteForSupplier(
  voteData: IVotingRequest,
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makePostRequest(
    serviceUrl,
    voteData,
    getAccessTokenSilently
  );
  return response;
}

const BasicControllerUrl: string = "https://localhost:5001/api/v1/Bids";

export interface GetBidsResponse {
  pageSize: number;
  numberOfPages: number;
  pageNumber: number;
  bidsPage: Bid[];
}
