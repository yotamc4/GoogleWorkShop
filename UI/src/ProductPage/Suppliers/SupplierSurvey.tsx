import * as React from "react";
import {
  ChoiceGroup,
  IChoiceGroupOption,
} from "office-ui-fabric-react/lib/ChoiceGroup";
import { Stack, Text, DefaultButton } from "@fluentui/react";
import {
  choiceGroupStyles,
  defaultButtonVoteStyles,
  textStyles,
  thankForYourVote,
  verticalGapStackTokens,
} from "./SupplierSurveyStyles";
import { useParams } from "react-router";
import { IVotingRequest } from "./SupplierSurvey.interface";
import { useAuth0 } from "@auth0/auth0-react";
import { voteForSupplier } from "../../Services/BidsControllerService";

export interface ISuppliersSurveyProps {
  supliersNames: IChoiceGroupOption[] | undefined;
  hasVoted: boolean | undefined;
}

export const SuppliersSurvey: React.FunctionComponent<ISuppliersSurveyProps> = ({
  supliersNames,
  hasVoted,
}) => {
  const { user, getAccessTokenSilently } = useAuth0();
  const { id } = useParams<{ id: string }>();
  //TODO: consume the supplier from the contextId!!!
  const [isVoteButtonClicked, setIsVoteButtonClicked] = React.useState<boolean>(
    hasVoted as boolean
  );
  const [selectedKey, setSelectedKey] = React.useState<string>();

  const onChange = React.useCallback(
    (
      evt?: React.FormEvent<HTMLElement | HTMLInputElement>,
      option: IChoiceGroupOption | undefined = undefined
    ) => {
      setSelectedKey(option?.key as string);
    },
    []
  );
  const onClickVoteButton = (): void => {
    const voteData: IVotingRequest = {
      bidId: id,
      buyerId: user.sub,
      votedSupplierId: selectedKey as string,
    };
    const url = `/${id}/vote`;
    voteForSupplier(voteData, url, getAccessTokenSilently);
    setIsVoteButtonClicked(true);
  };

  return isVoteButtonClicked ? (
    <Text styles={thankForYourVote}>Thank you for your Vote!</Text>
  ) : (
    <Stack tokens={verticalGapStackTokens}>
      <Text
        block={true}
        className="Bold"
        styles={textStyles}
        variant="xLargePlus"
      >
        Time to choose you're favorite supplier
      </Text>
      <ChoiceGroup
        styles={choiceGroupStyles}
        selectedKey={selectedKey}
        options={supliersNames}
        onChange={onChange}
      />
      <DefaultButton
        onClick={onClickVoteButton}
        text={"Vote"}
        primary
        styles={defaultButtonVoteStyles}
      ></DefaultButton>
    </Stack>
  );
};
