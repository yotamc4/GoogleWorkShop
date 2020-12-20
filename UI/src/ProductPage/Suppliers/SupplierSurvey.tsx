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
import axios from "axios";
import { useParams } from "react-router";
import { IVotingRequest } from "./SupplierSurvey.interface";

export interface ISuppliersSurveyProps {
  supliersNames: IChoiceGroupOption[] | undefined;
}

export const SuppliersSurvey: React.FunctionComponent<ISuppliersSurveyProps> = ({
  supliersNames,
}) => {
  const { id } = useParams<{ id: string }>();
  //TODO: consume the supplier from the contextId!!!
  const [isVoteButtonClicked, setIsVoteButtonClicked] = React.useState<boolean>(
    false
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
      buyerId: "EliLeherId",
      votedSupplierId: selectedKey as string,
    };
    axios
      .post(`https://localhost:5001/api/v1/bids/${id}/vote`, voteData)
      .then((response) => {
        console.log(response);
      })
      .catch((error) => {
        console.error(error);
      });
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
